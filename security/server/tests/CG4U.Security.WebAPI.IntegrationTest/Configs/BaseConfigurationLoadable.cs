using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using CG4U.Core.Services.ViewModels;
using CG4U.Security.WebAPI.IntegrationTest.DTO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace CG4U.Security.WebAPI.IntegrationTest.Configs
{
    public abstract class BaseConfigurationLoadable
    {
        protected RootLogin _login;

        public BaseConfigurationLoadable(RootLogin login)
        {
            _login = login;
        }

        private bool IsResponseAnError(string response)
        {
            if (response == null || response.Length == 0) return false;

            var rootError = JsonConvert.DeserializeObject<RootError>(response);
            if (rootError == null) return false;

            return rootError.error.Length > 0;
        }

        protected async Task<bool> IsNewItemAddedAsync(string requestUriGet, string postUri, StringContent postContent)
        {
            Environment.ClientApiSecurity.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _login.access_token);
            using (var responseGet = await Environment.ClientApiSecurity.
                   GetAsync(requestUriGet))
            {
                var responseGetContent = await responseGet.Content.ReadAsStringAsync();
                if (!IsResponseAnError(responseGetContent) && responseGetContent.Length == 0)
                {
                    using (var responsePost = await Environment.ServerApiSecurity
                        .CreateRequest(postUri)
                        .AddHeader("Authorization", "Bearer " + _login.access_token)
                        .And(request => request.Content = postContent)
                        .And(request => request.Method = HttpMethod.Post)
                        .PostAsync())
                    {
                        responsePost.EnsureSuccessStatusCode();
                        return true;
                    }
                }
                return false;
            }
        }
    }
}
