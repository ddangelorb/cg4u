using System;
using System.Collections.Generic;
using CG4U.Core.Common.Domain.Interfaces;
using CG4U.Core.Common.Domain.Models;
using CG4U.Core.Services.Authorization;
using CG4U.Core.Services.Services;
using CG4U.Core.Services.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace CG4U.Core.Services.InitialSetup
{
    public static class DataInitializer
    {
        public static void SeedData(
            ILogger logger,
            UserManager<IdentityUser> userManager,
            RoleManager<IdentityRole> roleManager,
            List<UserViewModel> userModels,
            IOptions<UserApi> userApi,
            IOptions<TokenDescriptor> tokenDescriptor)
        {
            SeedRoles(logger, roleManager);
            SeedUsers(logger, userManager, roleManager, userModels, userApi, tokenDescriptor);
        }

        private static void SeedRoles(ILogger logger, RoleManager<IdentityRole> roleManager)
        {
            var roles = new List<IdentityRole>()
            {
                new IdentityRole {
                    Name = UserRoles.Admin.ToString()
                },
                new IdentityRole {
                    Name = UserRoles.UserDonate.ToString()
                }
            };

            foreach (var role in roles)
            {
                if (!roleManager.RoleExistsAsync(role.Name).Result)
                {
                    var result = roleManager.CreateAsync(role).Result;
                    if (!result.Succeeded)
                    {
                        logger.LogError(string.Concat($"DataInitializer.SeedRoles:: Could not create '{role.Name}' role.", result.Errors.ToString()));
                        throw new Exception($"Could not create '{role.Name}' role.");
                    }

                }
                //await roleManager.AddClaimAsync(adminRole, new Claim(CustomClaimTypes.Permission, "projects.view"));
                //await roleManager.AddClaimAsync(adminRole, new Claim(CustomClaimTypes.Permission, "projects.create"));
                //await roleManager.AddClaimAsync(adminRole, new Claim(CustomClaimTypes.Permission, "projects.update"));
            }
        }

        private static void SeedUsers(
            ILogger logger,
            UserManager<IdentityUser> userManager,
            RoleManager<IdentityRole> roleManager,
            List<UserViewModel> userModels,
            IOptions<UserApi> userApi,
            IOptions<TokenDescriptor> tokenDescriptor)
        {
            //var userIdentityAdm = new IdentityUser();

            foreach (var userModel in userModels)
            {
                if (userManager.FindByEmailAsync(userModel.Email).Result != null)
                    return;
                
                if (!roleManager.RoleExistsAsync(UserRoles.Admin.ToString()).Result
                    || !roleManager.RoleExistsAsync(UserRoles.UserDonate.ToString()).Result)
                {
                    logger.LogError($"The roles have not yet been created.");
                    throw new Exception($"The roles have not yet been created.");
                }

                var user = new IdentityUser
                {
                    UserName = userModel.Email,
                    Email = userModel.Email
                };

                var resultCreateUser = userManager.CreateAsync(user, userModel.Password);
                if (!resultCreateUser.Result.Succeeded)
                {
                    logger.LogError(resultCreateUser.Exception, $"The {userModel.Email} user has not yet been created.");
                    throw new Exception($"The {userModel.Email} user has not yet been created.");
                }

                foreach (var role in userModel.Roles)
                {
                    var resultAddRole = userManager.AddToRoleAsync(user, role.ToString());
                    if (!resultAddRole.Result.Succeeded)
                    {
                        logger.LogError(resultAddRole.Exception, $"The {userModel.Email} user has not yet been associated with {role.ToString()}.");
                        throw new Exception($"The {userModel.Email} user has not yet been associated with {role.ToString()}.");
                    }
                    //userIdentityAdm = role == UserRoles.Admin ? user : userIdentityAdm;
                }

                var token = userManager.GenerateEmailConfirmationTokenAsync(user).Result;
                var resultEmailConfirmation = userManager.ConfirmEmailAsync(user, token);
                if (!resultEmailConfirmation.Result.Succeeded)
                {
                    logger.LogError(resultEmailConfirmation.Exception, $"The {userModel.Email} user has not yet been confirmed with its email.");
                    throw new Exception($"The {userModel.Email} user has not yet been confirmed with its email.");
                }

                userModel.IdUserIdentity = user.Id;
                //var adapter = new UserAdapter(userIdentityAdm, userApi, userManager, tokenDescriptor, logger);
                var adapter = new UserAdapter(userApi, userManager, tokenDescriptor, null);
                var resultUserAdded = adapter.AddDbAsync(user, userModel);
                if (!resultUserAdded.Result)
                    throw new Exception($"The {userModel.Email} user has not yet been created on CG4UCore..Users.");
            }
        }
    }
}
