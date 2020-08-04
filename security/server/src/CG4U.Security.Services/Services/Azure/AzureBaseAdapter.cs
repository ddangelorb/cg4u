using System;
using System.Net.Http;
using CG4U.Security.Services.Data;
using Microsoft.Extensions.Logging;

namespace CG4U.Security.Services.Services.Azure
{
    public abstract class AzureBaseAdapter
    {
        protected ILogger _logger;

        public AzureBaseAdapter(ILogger logger)
        {
            _logger = logger;
        }

        protected HttpClient GetAzureApiClient(ConnectionApiData connectionApiData)
        {
            var apiClient = new HttpClient();

            apiClient.BaseAddress = new Uri(connectionApiData.Location);
            apiClient.DefaultRequestHeaders.Add(
                "Ocp-Apim-Subscription-Key", connectionApiData.SubscriptionKey);

            return apiClient;
        }
    }
}
