using System.Collections.Generic;
using System.Threading.Tasks;
using CG4U.Core.Common.Domain.Models;
using CG4U.Core.Services.Authorization;
using CG4U.Core.Services.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace CG4U.Core.Services.Services
{
    public class UserAdapterTests : UserAdapter
    {
        private Dictionary<string, int> _userDictionary;

        public UserAdapterTests(IOptions<UserApi> userApiOption,
                           UserManager<IdentityUser> userManager,
                           IOptions<TokenDescriptor> tokenDescriptorOption,
                           ILogger<UserAdapterTests> logger)
            : base(userApiOption, userManager, tokenDescriptorOption, logger)
        {
            _userDictionary = new Dictionary<string, int>();
            _userDictionary.Add("71cefb2b-09c0-4f34-b587-d5619682d98c", 1);
            _userDictionary.Add("3c1aa042-562e-4a6f-b38b-02e7c6b52610", 2);
            _userDictionary.Add("3ce21204-6633-4ffc-9fda-c979193f7686", 3);
        }

        public override async Task<UserViewModel> GetDbByIdentityAsync(IdentityUser identityUser)
        {
            int idUser;
            if (!_userDictionary.TryGetValue(identityUser.Id, out idUser))
                return null;
            
            var uvm = new UserViewModel()
            {
                Id = idUser,
                IdUserIdentity = identityUser.Id,
                IdSystems = 1,
                IdLanguages = 1,
                Active = 1
            };

            return await Task.FromResult(uvm);
        }

        public override async Task<bool> IsUserHaveAccessSystem(IdentityUser identityUser, int idSystems)
        {
            return await Task.FromResult(true);
        }
    }
}
