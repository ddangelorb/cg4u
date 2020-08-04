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
    public class VideoCamerasLoadable : BaseConfigurationLoadable, IConfigurationLoadable<List<VideoCameraViewModel>>
    {
        private List<AnalyzeRequestViewModel> _analyzeRequests;

        public VideoCamerasLoadable(RootLogin login, List<AnalyzeRequestViewModel> analyzeRequests)
            : base(login)
        {
            _analyzeRequests = analyzeRequests;
        }

        public async Task<List<VideoCameraViewModel>> GetResultLoaded(IConfigurationRoot config)
        {
            var videoCameras = new List<VideoCameraViewModel>();
            config.GetSection("VideoCameras").Bind(videoCameras);
            foreach (var videoCamera in videoCameras)
            {
                using (var postContent = new StringContent(JsonConvert.SerializeObject(videoCamera), Encoding.UTF8, "application/json"))
                {
                    var requestUriGet = $"Configuration/GetVideoCameraByIdAsync/{videoCamera.Id}";
                    var postUri = "Configuration/AddVideoCameraAsync";

                    var isNewItem = await IsNewItemAddedAsync(requestUriGet, postUri, postContent);
                    if (isNewItem)
                    {
                        await AddAnalyzeRequestConnectionVideoCameraAsync(videoCamera.Id);
                    }
                }
            }
            return videoCameras;
        }

        private async Task AddAnalyzeRequestConnectionVideoCameraAsync(int idVideoCamera)
        {
            foreach (var analyzeRequest in _analyzeRequests)
            {
                var viewModel = new AnalyzeRequestVideoCameraViewModel()
                {
                    IdAnalyzesRequests = analyzeRequest.Id,
                    IdVideoCameras = idVideoCamera
                };

                using (var postContent = new StringContent(JsonConvert.SerializeObject(viewModel), Encoding.UTF8, "application/json"))
                using (var response = await Environment.ServerApiSecurity
                    .CreateRequest("Configuration/AddAnalyzeRequestConnectionVideoCameraAsync")
                    .AddHeader("Authorization", "Bearer " + _login.access_token)
                    .And(request => request.Content = postContent)
                    .And(request => request.Method = HttpMethod.Post)
                    .PostAsync())
                {
                    response.EnsureSuccessStatusCode();
                }
            }
        }
    }
}
