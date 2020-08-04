using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using CG4U.Core.Services.ViewModels;
using CG4U.Donate.WebAPI.IntegrationTest.DTO;
using Newtonsoft.Json;

namespace CG4U.Donate.WebAPI.IntegrationTest
{
    public class UserUtils
    {
        public static async Task<object> DoRegister(HttpClient client, UserViewModel registerViewModel)
        {
            using (var postContent = new StringContent(JsonConvert.SerializeObject(registerViewModel), Encoding.UTF8, "application/json"))
            using (var response = await client.PostAsync("Account/RegisterAsync", postContent))
            {
                var postResult = await response.Content.ReadAsStringAsync();
                var userResult = JsonConvert.DeserializeObject<RootRegister>(postResult);

                return userResult;
            }
        }

        public static async Task<RootLogin> DoLogin(HttpClient client, UserViewModel loginViewModel)
        {
            using (var postContent = new StringContent(JsonConvert.SerializeObject(loginViewModel), Encoding.UTF8, "application/json"))
            using (var response = await client.PostAsync("Account/LoginAsync", postContent))
            {
                var postResult = await response.Content.ReadAsStringAsync();
                var userResult = JsonConvert.DeserializeObject<RootLogin>(postResult);

                return userResult;
            }
        }
    }
}
