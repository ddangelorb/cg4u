using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using CG4U.Security.WebAPI.IntegrationTest.DTO;
using CG4U.Security.WebAPI.ViewModels;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace CG4U.Security.WebAPI.IntegrationTest.Configs
{
    public class AlertsConfigurationLoadable : BaseConfigurationLoadable, IConfigurationLoadable<List<AlertViewModel>>
    {
        public AlertsConfigurationLoadable(RootLogin login)
            : base(login)
        {
        }

        public async Task<List<AlertViewModel>> GetResultLoaded(IConfigurationRoot config)
        {
            var alerts = new List<AlertViewModel>();
            config.GetSection("Alerts").Bind(alerts);
            foreach (var alert in alerts)
            {
                using (var postContent = new StringContent(JsonConvert.SerializeObject(alert), Encoding.UTF8, "application/json"))
                {
                    var requestUriGet = $"Configuration/GetAlertByIdAsync/{alert.Id}";
                    var postUri = "Configuration/AddAlertAsync";
                    await IsNewItemAddedAsync(requestUriGet, postUri, postContent);
                }
            }
            return alerts;
        }
    }
}
