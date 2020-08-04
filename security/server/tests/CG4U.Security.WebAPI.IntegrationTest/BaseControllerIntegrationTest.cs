using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using CG4U.Core.Services.ViewModels;
using CG4U.Security.WebAPI.IntegrationTest.Configs;
using CG4U.Security.WebAPI.IntegrationTest.DTO;
using CG4U.Security.WebAPI.ViewModels;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace CG4U.Security.WebAPI.IntegrationTest
{
    public abstract class BaseControllerIntegrationTest
    {
        protected List<UserViewModel> _logins;
        protected RootLogin _loginAdm;
        protected List<AnalyzeRequestViewModel> _analyzeRequests;
        protected List<AlertViewModel> _alerts;
        protected PersonGroupViewModel _personGroup;
        protected List<PersonViewModel> _persons;
        protected List<VideoCameraViewModel> _videoCameras;

        public BaseControllerIntegrationTest()
        {
            _logins = new List<UserViewModel>();
            Environment.SetupEnvironment();
            SetConfigVariables().Wait();
        }

        private async Task SetConfigVariables()
        {
            var configurationBuilder = new ConfigurationBuilder();
            var path = Path.Combine(Directory.GetCurrentDirectory(), "appsettings.Development.json");
            configurationBuilder.AddJsonFile(path, false);

            var appsettingsConfig = configurationBuilder.Build();

            appsettingsConfig.GetSection("Logins").Bind(_logins);
            _loginAdm = await Environment.DoLogin(Environment.ClientApiAuth, _logins[0]);

            _analyzeRequests = await GetAnalyzesRequestsAsync(appsettingsConfig);

            _alerts = await GetAlertsAsync(appsettingsConfig);

            _personGroup = await GetPersonGroupAsync(appsettingsConfig);

            _videoCameras = await GetVideoCamerasAsync(appsettingsConfig);

            _persons = await GetPersonsAsync(appsettingsConfig);
        }

        private async Task<List<AnalyzeRequestViewModel>> GetAnalyzesRequestsAsync(IConfigurationRoot appsettingsConfig)
        {
            var loadable = new AnalyzeRequestsConfigurationLoadable(_loginAdm);
            return await loadable.GetResultLoaded(appsettingsConfig);
        }

        private async Task<List<AlertViewModel>> GetAlertsAsync(IConfigurationRoot appsettingsConfig)
        {
            var loadable = new AlertsConfigurationLoadable(_loginAdm);
            return await loadable.GetResultLoaded(appsettingsConfig);
        }

        private async Task<PersonGroupViewModel> GetPersonGroupAsync(IConfigurationRoot appsettingsConfig)
        {
            var loadable = new PersonGroupLoadable(_loginAdm, _alerts);
            return await loadable.GetResultLoaded(appsettingsConfig);
        }

        private async Task<List<PersonViewModel>> GetPersonsAsync(IConfigurationRoot appsettingsConfig)
        {
            var loadable = new PersonsLoadable(_loginAdm);
            return await loadable.GetResultLoaded(appsettingsConfig);
        }

        private async Task<List<VideoCameraViewModel>> GetVideoCamerasAsync(IConfigurationRoot appsettingsConfig)
        {
            var loadable = new VideoCamerasLoadable(_loginAdm, _analyzeRequests);
            return await loadable.GetResultLoaded(appsettingsConfig);
        }
    }
}
