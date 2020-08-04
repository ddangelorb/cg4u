using System.IO;
using System.Net.Http;
using CG4U.Auth.WebAPI;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;

namespace CG4U.Donate.WebAPI.IntegrationTest
{
    public class Environment
    {
        public static TestServer ServerApiDonate { get; set; }
        public static HttpClient ClientApiDonate { get; set; }
        public static TestServer ServerApiAuth { get; set; }
        public static HttpClient ClientApiAuth { get; set; }

        public static void SetupEnvironment()
        {
            if (ServerApiDonate == null) 
            {
                ServerApiDonate = new TestServer(
                    new WebHostBuilder()
                        .UseKestrel()
                        .UseEnvironment("Development")
                        .UseContentRoot(Directory.GetCurrentDirectory())
                        .UseUrls("http://localhost:8285")
                        .UseStartup<StartupDonateTests>()
                        .ConfigureAppConfiguration((builderContext, config) =>
                        {
                            IHostingEnvironment env = builderContext.HostingEnvironment;
                            config.AddJsonFile("appsettings.Development.json", optional: false, reloadOnChange: true);
                        }));

                ClientApiDonate = ServerApiDonate.CreateClient();
            }

            if (ServerApiAuth == null) 
            {
                ServerApiAuth = new TestServer(
                    new WebHostBuilder()
                        .UseKestrel()
                        .UseEnvironment("Development")
                        .UseContentRoot(Directory.GetCurrentDirectory())
                        .UseUrls("http://localhost:8286")
                        .UseStartup<StartupAuthTests>()
                        .ConfigureAppConfiguration((builderContext, config) =>
                        {
                            IHostingEnvironment env = builderContext.HostingEnvironment;
                            config.AddJsonFile("appsettings.Development.json", optional: false, reloadOnChange: true);
                        }));

                ClientApiAuth = ServerApiAuth.CreateClient();
            }
        }
    }
}
