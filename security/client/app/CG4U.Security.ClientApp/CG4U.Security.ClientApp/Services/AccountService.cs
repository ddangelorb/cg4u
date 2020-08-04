using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using CG4U.Security.ClientApp.Services.Interfaces;
using CG4U.Security.ClientApp.Services.Roots;
using Newtonsoft.Json;

namespace CG4U.Security.ClientApp.Services
{
    public class AccountService : IAccountService
    {
        HttpClient client;

        public AccountService()
        {
            client = new HttpClient();
            client.BaseAddress = new Uri($"http://localhost:5001/");
        }

        public async Task<RootLogin> LoginAsync(ViewModels.LoginViewModel loginViewModel)
        {
            var postContent = GetAccountPostContent(loginViewModel);
            var response = await client.PostAsync("Account/LoginAsync", postContent);

            var postResult = await response.Content.ReadAsStringAsync();
            var userResult = JsonConvert.DeserializeObject<RootLogin>(postResult);

            return userResult;
        }

        public Task<RootRegister> RegisterAsync(ViewModels.LoginViewModel loginViewModel)
        {
            throw new NotImplementedException();
        }

        private StringContent GetAccountPostContent(ViewModels.LoginViewModel loginViewModel)
        {
            var content = new
            {
                Email = loginViewModel.Email,
                Password = loginViewModel.Password,
                PasswordConfirmation = loginViewModel.Password,
                IdLanguages = App.IdLanguages,
                IdSystems = App.IdSystems
            };

            return new StringContent(JsonConvert.SerializeObject(content), Encoding.UTF8, "application/json");
        }
    }
}
