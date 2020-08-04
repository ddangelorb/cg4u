using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CG4U.Security.ClientApp.Services.Interfaces;
using CG4U.Security.ClientApp.Services.Roots;
using CG4U.Security.ClientApp.ViewModels;

namespace CG4U.Security.ClientApp.Services.Mocks
{
    public class MockAccountService : IAccountService
    {
        public Task<RootLogin> LoginAsync(LoginViewModel loginViewModel)
        {
            var rootUser = GetRootUser(loginViewModel);
            var token = rootUser == null ? null : "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiIzYzFhYTA0Mi01NjJlLTRhNmYtYjM4Yi0wMmU3YzZiNTI2MTAiLCJlbWFpbCI6ImRhbmllbGRyYkBob3RtYWlsLmNvbSIsImp0aSI6IjIzNWRlMjNhLTVlYjEtNDE1Yi05YTJkLTk4YzMyYzYzZTVhMyIsImlhdCI6MTUyNjI1OTMwNSwiaHR0cDovL3NjaGVtYXMubWljcm9zb2Z0LmNvbS93cy8yMDA4LzA2L2lkZW50aXR5L2NsYWltcy9yb2xlIjoiVXNlckRvbmF0ZSIsImV4cCI6MTUyNjI3NzMwNCwiaXNzIjoiQ0c0VS5BdXRoLldlYkFQSS5Ub2tlblNlcnZlciIsImF1ZCI6Imh0dHA6Ly93d3cuY2c0dS5jb20vYXV0aCJ9.kMIKe5G1s1m71PTXhY4MAdV1hP5y-Hh_MwuLjf1bhiM";

            return Task.FromResult(
                new RootLogin()
                {
                    access_token = token,
                    expires_in = DateTime.Now.AddDays(1),
                    user = rootUser
                }
            );
        }

        public Task<RootRegister> RegisterAsync(LoginViewModel loginViewModel)
        {
            throw new NotImplementedException();
        }

        public RootUser GetRootUser(LoginViewModel loginViewModel)
        {
            if (loginViewModel == null
                || loginViewModel.Email == null
                || loginViewModel.Password == null)
                return null;

            if (loginViewModel.Email == "danieldrb@hotmail.com"
               && loginViewModel.Password == "123")
            {
                return new RootUser()
                {
                    id = 2,
                    idUserIdentity = "3c1aa042-562e-4a6f-b38b-02e7c6b52610",
                    firstName = "Daniel",
                    surName = "Barros",
                    email = "daniedrb@hotmail.com",
                    idSystem = 1,
                    idLanguage = 1,
                    claims = new List<RootClaim>()
                    {
                        new RootClaim(){ type = "sub", value = "3c1aa042-562e-4a6f-b38b-02e7c6b52610"},
                        new RootClaim(){ type = "email", value = "danieldrb@hotmail.com"},
                        new RootClaim(){ type = "jti", value = "67791243-ac16-442a-8acc-ba88b006d2af"},
                        new RootClaim(){ type = "iat", value = "1526301859"},
                        new RootClaim(){ type = "http://schemas.microsoft.com/ws/2008/06/identity/claims/role", value = "UserDonate"}
                    }.ToArray()
                };
            }
            else
                return null;
        }
    }
}
