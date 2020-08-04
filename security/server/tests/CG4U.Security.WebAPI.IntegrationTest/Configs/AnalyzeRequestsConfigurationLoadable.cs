using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using CG4U.Core.Services.ViewModels;
using CG4U.Security.WebAPI.IntegrationTest.DTO;
using CG4U.Security.WebAPI.ViewModels;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace CG4U.Security.WebAPI.IntegrationTest.Configs
{
    public class AnalyzeRequestsConfigurationLoadable : BaseConfigurationLoadable, IConfigurationLoadable<List<AnalyzeRequestViewModel>>
    {
        public AnalyzeRequestsConfigurationLoadable(RootLogin login)
            : base(login)
        {
        }

        public async Task<List<AnalyzeRequestViewModel>> GetResultLoaded(IConfigurationRoot config)
        {
            var analyzeRequests = new List<AnalyzeRequestViewModel>();
            config.GetSection("AnalyzeRequests").Bind(analyzeRequests);
            foreach (var analyzeRequest in analyzeRequests)
            {
                using (var postContent = new StringContent(JsonConvert.SerializeObject(analyzeRequest), Encoding.UTF8, "application/json"))
                {
                    var requestUriGet = $"Configuration/GetAnalyzeRequestByIdAsync/{analyzeRequest.Id}";
                    var postUri = "Configuration/AddAnalyzeRequestAsync";
                    await IsNewItemAddedAsync(requestUriGet, postUri, postContent);
                }
            }
            return analyzeRequests;
        }
    }
}
