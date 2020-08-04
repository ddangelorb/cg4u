using System;
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
    public class PersonGroupLoadable : BaseConfigurationLoadable, IConfigurationLoadable<PersonGroupViewModel>
    {
        private List<AlertViewModel> _alerts;

        public PersonGroupLoadable(RootLogin login, List<AlertViewModel> alerts)
            : base(login)
        {
            _alerts = alerts;
        }

        public async Task<PersonGroupViewModel> GetResultLoaded(IConfigurationRoot config)
        {
            var personGroup = new PersonGroupViewModel();
            config.GetSection("PersonGroup").Bind(personGroup);
            using (var postContent = new StringContent(JsonConvert.SerializeObject(personGroup), Encoding.UTF8, "application/json"))
            {
                var requestUriGet = $"Person/GetPersonGroupByIdAsync/{personGroup.Id}";
                var postUri = "Person/AddPersonGroupAsync";

                var isNewItem = await IsNewItemAddedAsync(requestUriGet, postUri, postContent);
                if (isNewItem)
                {
                    await AddAlertConnectionPersonGroupAsync(personGroup.Id);
                    await TrainPersonGroupAsync(postContent);
                }
            }
            return personGroup;
        }

        private async Task AddAlertConnectionPersonGroupAsync(int idPersonGroups)
        {
            foreach (var alert in _alerts)
            {
                var alertPersonGroupViewModel = new AlertPersonGroupViewModel
                {
                    IdPersonGroups = idPersonGroups,
                    IdAlerts = alert.Id
                };

                using (var postContent = new StringContent(JsonConvert.SerializeObject(alertPersonGroupViewModel), Encoding.UTF8, "application/json"))
                using (var response = await Environment.ServerApiSecurity
                    .CreateRequest("Configuration/AddAlertConnectionPersonGroupAsync")
                    .AddHeader("Authorization", "Bearer " + _login.access_token)
                    .And(request => request.Content = postContent)
                    .And(request => request.Method = HttpMethod.Post)
                    .PostAsync())
                {
                    response.EnsureSuccessStatusCode();
                }
            }
        }

        private async Task TrainPersonGroupAsync(StringContent postContent)
        {
            using (var response = await Environment.ServerApiSecurity
                .CreateRequest("Person/TrainPersonGroupAsync")
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
