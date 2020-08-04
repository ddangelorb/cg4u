using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using CG4U.Security.Services.Data;
using CG4U.Security.Services.Interfaces;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.ProjectOxford.Face;
using Microsoft.ProjectOxford.Face.Contract;
using Newtonsoft.Json;

namespace CG4U.Security.Services.Services.Azure
{
    public class AzureFaceAdapter : AzureBaseAdapter, IFaceAdapter
    {
        private AzureApiData _azureApiData;

        public AzureFaceAdapter(IOptions<AzureApiData> azureApiData, ILogger logger)
            : base(logger)
        {
            _azureApiData = azureApiData.Value;
        }

        public async Task<string> IdentifyFaceAsync(byte[] image, string personGroupId, ConnectionApiData connectionApiData)
        {
            var faceIds = await GetFaceIds(image, connectionApiData);
            if (faceIds == null) return null;

            //var ss = await faceClient.IdentifyAsync(personGroupId, faceIds.ToArray());
            var uri = _azureApiData.IdentifyFaceUri;
            var data = new { faceIds = faceIds, personGroupId = personGroupId };
            using (var azureClientApi = GetAzureApiClient(connectionApiData))
            using (var postContent = new StringContent(JsonConvert.SerializeObject(data), Encoding.UTF8, "application/json"))
            using (var response = await azureClientApi.PostAsync(uri, postContent))
            {
                if (!response.IsSuccessStatusCode)
                {
                    _logger.LogError(string.Concat("AzureFaceAdapter.CreatePersonGroupAsync::Cannot create person group with azureApi: ", response.ToString()));
                    return null;
                }
                return await response.Content.ReadAsStringAsync();
            }
        }

        public async Task CreatePersonGroupAsync(string id, string name, ConnectionApiData connectionApiData)
        {
            using (var faceClient = new FaceServiceClient(connectionApiData.SubscriptionKey, connectionApiData.Location))
            {
                await faceClient.CreatePersonGroupAsync(id, name);
            }
        }

        public async Task<string> CreatePersonGroupPersonAsync(string personGroupId, string name, ConnectionApiData connectionApiData)
        {
            using (var faceClient = new FaceServiceClient(connectionApiData.SubscriptionKey, connectionApiData.Location))
            {
                var result = await faceClient.CreatePersonAsync(personGroupId, name);
                return result.PersonId.ToString();
            }
        }

        public async Task<string> AddFacePersonGroupPersonAsync(string personGroupId, string personId, byte[] image, ConnectionApiData connectionApiData)
        {
            using (var faceClient = new FaceServiceClient(connectionApiData.SubscriptionKey, connectionApiData.Location))
            using (var ms = new MemoryStream(image))
            {
                var result = await faceClient.AddPersonFaceAsync(personGroupId, new Guid(personId), ms);
                return result.PersistedFaceId.ToString();
            }
        }

        public async Task TrainPersonGroupAsync(string personGroupId, ConnectionApiData connectionApiData)
        {
            using (var faceClient = new FaceServiceClient(connectionApiData.SubscriptionKey, connectionApiData.Location))
            {
                await faceClient.TrainPersonGroupAsync(personGroupId);
            }
        }

        public async Task<string> GetTrainingStatusPersonGroupAsync(string personGroupId, ConnectionApiData connectionApiData)
        {
            using (var faceClient = new FaceServiceClient(connectionApiData.SubscriptionKey, connectionApiData.Location))
            {
                var trainingStatus = await faceClient.GetPersonGroupTrainingStatusAsync(personGroupId);
                return trainingStatus.Status.ToString();
            }
        }

        private async Task<List<Guid>> GetFaceIds(byte[] image, ConnectionApiData connectionApiData)
        {
            using (var msImage = new MemoryStream(image))
            using (var faceClient = new FaceServiceClient(connectionApiData.SubscriptionKey, connectionApiData.Location))
            {
                Face[] faces = await faceClient.DetectAsync(msImage);
                var faceIds = faces.Count() > 0 ? faces.Select(f => f.FaceId).ToList() : null;

                if (faceIds == null)
                    _logger.LogError(string.Concat("AzureFaceAdapter.GetFaceIds::Cannot get faces from image"));

                return faceIds;
            }            
        }
    }
}
