using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using CG4U.Core.Common.Domain.Models;
using CG4U.Core.Services.Interfaces;
using CG4U.Core.Services.ViewModels;
using Microsoft.AspNetCore.Identity;
using Newtonsoft.Json;
using System.Security.Claims;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using System.IdentityModel.Tokens.Jwt;
using CG4U.Core.Services.Authorization;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;

namespace CG4U.Core.Services.Services
{
    public class UserAdapter : IUserAdapter
    {
        public ICollection<string> Errors { get; private set; }

        protected UserApi _userApi;
        protected UserManager<IdentityUser> _userManager;
        protected TokenDescriptor _tokenDescriptor;
        protected ILogger _logger;

        protected static long ToUnixEpochDate(DateTime date)
        => (long)Math.Round((date.ToUniversalTime() - new DateTimeOffset(1970, 1, 1, 0, 0, 0, TimeSpan.Zero)).TotalSeconds);

        public UserAdapter(IOptions<UserApi> userApiOption, 
                           UserManager<IdentityUser> userManager, 
                           IOptions<TokenDescriptor> tokenDescriptorOption, 
                           ILogger<UserAdapter> logger)
        {
            Errors = new List<string>();
            _userApi = userApiOption.Value;
            _userManager = userManager;
            _tokenDescriptor = tokenDescriptorOption.Value;
            _logger = logger;
        }

        public virtual async Task<UserViewModel> GetDbByIdentityAsync(IdentityUser identityUser)
        {
            if (identityUser == null)
            {
                AddError("UserAdapter.GetDbByIdentity::User Identity not found.");
                return null;
            }

            using (var client = await GetUserApiClientAsync(identityUser))
            using (var response = await client.GetAsync(string.Format(_userApi.GetUri, identityUser.Id)))                
            {
                if (!response.IsSuccessStatusCode)
                {
                    AddError(string.Concat("UserAdapter.GetDbByIdentity::Cannot get client with userApi. Status code:", response.StatusCode.ToString()));
                    return null;
                }

                var responseString = await response.Content.ReadAsStringAsync();
                if (responseString == null)
                {
                    AddError("UserAdapter.GetDbByIdentity::Cannot read client with userApi");
                    return null;
                }

                return JsonConvert.DeserializeObject<UserViewModel>(responseString);
            }
        }

        public virtual async Task<bool> IsUserHaveAccessSystem(IdentityUser identityUser, int idSystems)
        {
            if (identityUser == null)
            {
                AddError("UserAdapter.IsUserHaveAccessSystem::User Identity not found.");
                return false;
            }

            using (var client = await GetUserApiClientAsync(identityUser))
            using (var response = await client.GetAsync(string.Format(_userApi.IsUserHasAccess, identityUser.Id, idSystems)))                
            {
                if (!response.IsSuccessStatusCode)
                {
                    AddError(string.Concat("UserAdapter.IsUserHaveAccessSystem::Cannot get method result. Status code:", response.StatusCode.ToString()));
                    return false;
                }

                var responseString = await response.Content.ReadAsStringAsync();
                if (responseString == null)
                {
                    AddError("UserAdapter.IsUserHaveAccessSystem::Cannot read client with userApi");
                    return false;
                }

                return responseString.ToLower().Equals("true");
            }
        }

        public async Task<bool> AddDbAsync(IdentityUser identityUser, UserViewModel model)
        {
            using (var client = await GetUserApiClientAsync(identityUser))
            using (var postContent = new StringContent(JsonConvert.SerializeObject(model), Encoding.UTF8, "application/json"))                
            using (var response = await client.PostAsync(_userApi.AddUri, postContent))                
            {
                if (!response.IsSuccessStatusCode)
                {
                    AddError(string.Concat("UserAdapter.AddDb.AddUri::Cannot add client with userApi: ", response.ToString()));
                    return false;
                }

                return true;
            }
        }

        public async Task<bool> AddSystemDbAsync(IdentityUser identityUser, UserViewModel model)
        {
            var uSystemViewModel = new { IdUserIdentity = model.IdUserIdentity, IdSystems = model.IdSystems };

            using (var client = await GetUserApiClientAsync(identityUser))
            using (var postContentUSystem = new StringContent(JsonConvert.SerializeObject(uSystemViewModel), Encoding.UTF8, "application/json"))                
            using (var responseUSystem = await client.PostAsync(_userApi.AddSystemUri, postContentUSystem))                
            {
                if (!responseUSystem.IsSuccessStatusCode)
                {
                    AddError(string.Concat("UserAdapter.AddSystemDb::Cannot add system client with userApi: ", responseUSystem.ToString()));
                    return false;
                }

                return true;
            }
        }

        public async Task<bool> EnableDbAsync(IdentityUser identityUser, int idSystems)
        {
            if (identityUser == null)
            {
                AddError("UserAdapter.EnableDbAsync::User Identity not found.");
                return false;
            }

            using (var client = await GetUserApiClientAsync(identityUser))
            using (var postContent = new StringContent(JsonConvert.SerializeObject(new { IdUserIdentiy = identityUser.Id, IdSystems = idSystems }), Encoding.UTF8, "application/json"))
            using (var response = await client.PutAsync(_userApi.EnableUri, postContent))                
            {
                if (!response.IsSuccessStatusCode)
                {
                    AddError(string.Concat("UserAdapter.EnableDbAsync::Cannot enable client with userApi : ", response.ToString()));
                    return false;
                }
            }

            return true;
        }

        public async Task<object> GenerateJwtTokenAsync(IdentityUser identityUser, UserViewModel login)
        {
            var claims = await GetValidClaims(identityUser);

            if (identityUser == null)
            {
                AddError("UserAdapter.GenerateJwtToken::User Identity not found.");
                return null;
            }

            var encodedJwt = GetEncodedJwtToken(claims);
            var response = new
            {
                access_token = encodedJwt,
                expires_in = DateTime.Now.AddMinutes(_tokenDescriptor.MinutesValid),
                user = new
                {
                    id = login.Id,
                    idUserIdentity = identityUser.Id,
                    firstName = login.FirstName,
                    surName = login.SurName,
                    email = identityUser.Email,
                    idSystem = login.IdSystems,
                    idLanguage = login.IdLanguages,
                    claims = claims.Select(c => new { c.Type, c.Value })
                }
            };

            return response;
        }

        public ICollection<string> GetErrors()
        {
            return Errors;
        }

        protected async Task<HttpClient> GetUserApiClientAsync(IdentityUser identityUser)
        {
            var claims = await GetValidClaims(identityUser);
            var authKey = GetEncodedJwtToken(claims);

            var userApiClient = new HttpClient();
            userApiClient.BaseAddress = new Uri(_userApi.BaseAddress);
            userApiClient.DefaultRequestHeaders.Accept.Clear();
            userApiClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            userApiClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + authKey);
            return userApiClient;
        }

        protected string GetEncodedJwtToken(List<Claim> claims)
        {
            var signingConf = new SigningCredentialsConfiguration();
            var token = new JwtSecurityToken(
                issuer: _tokenDescriptor.Issuer,
                audience: _tokenDescriptor.Audience,
                claims: claims,
                expires: DateTime.Now.AddMinutes(_tokenDescriptor.MinutesValid),
                signingCredentials: signingConf.SigningCredentials
            );

            var handler = new JwtSecurityTokenHandler();
            return handler.WriteToken(token);
        }

        protected async Task<List<Claim>> GetValidClaims(IdentityUser identityUser)
        {
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, identityUser.Id),
                new Claim(JwtRegisteredClaimNames.Email, identityUser.Email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Iat, ToUnixEpochDate(DateTime.UtcNow).ToString(), ClaimValueTypes.Integer64)
            };

            var userClaims = await _userManager.GetClaimsAsync(identityUser);
            claims.AddRange(userClaims);

            var userRoles = await _userManager.GetRolesAsync(identityUser);
            foreach (var userRole in userRoles)
            {
                claims.Add(new Claim(ClaimTypes.Role, userRole));
            }

            return claims;
        }

        protected void AddError(string message)
        {
            _logger.LogError(message);
            Errors.Add(message);
        }
    }
}
