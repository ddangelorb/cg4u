using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using CG4U.Auth.WebAPI;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using System.Text;
using CG4U.Core.Services.ViewModels;
using Newtonsoft.Json;
using CG4U.Security.WebAPI.IntegrationTest.DTO;

namespace CG4U.Security.WebAPI.IntegrationTest
{
    public class Environment
    {
        public static TestServer ServerApiSecurity { get; set; }
        public static HttpClient ClientApiSecurity { get; set; }
        public static TestServer ServerApiAuth { get; set; }
        public static HttpClient ClientApiAuth { get; set; }

        public static void SetupEnvironment()
        {
            if (ServerApiSecurity == null)
            {
                ServerApiSecurity = new TestServer(
                    new WebHostBuilder()
                        .UseKestrel()
                        .UseEnvironment("Development")
                        .UseContentRoot(Directory.GetCurrentDirectory())
                        .UseUrls("http://localhost:8385")
                        .UseStartup<StartupSecurityTests>()
                        .ConfigureAppConfiguration((builderContext, config) =>
                        {
                            IHostingEnvironment env = builderContext.HostingEnvironment;
                            config.AddJsonFile("appsettings.Development.json", optional: false, reloadOnChange: true);
                        }));

                ClientApiSecurity = ServerApiSecurity.CreateClient();
            }

            if (ServerApiAuth == null)
            {
                ServerApiAuth = new TestServer(
                    new WebHostBuilder()
                        .UseKestrel()
                        .UseEnvironment("Development")
                        .UseContentRoot(Directory.GetCurrentDirectory())
                        .UseUrls("http://localhost:8386")
                        .UseStartup<StartupAuthTests>()
                        .ConfigureAppConfiguration((builderContext, config) =>
                        {
                            IHostingEnvironment env = builderContext.HostingEnvironment;
                            config.AddJsonFile("appsettings.Development.json", optional: false, reloadOnChange: true);
                        }));

                ClientApiAuth = ServerApiAuth.CreateClient();
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
