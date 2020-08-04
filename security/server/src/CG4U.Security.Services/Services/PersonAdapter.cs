using System;
using System.Threading.Tasks;
using CG4U.Security.Domain.Configuration;
using CG4U.Security.Domain.Configuration.Repository;
using CG4U.Security.Domain.Persons;
using CG4U.Security.Services.Data;
using CG4U.Security.Services.Interfaces;
using Microsoft.Extensions.Logging;

namespace CG4U.Security.Services.Services
{
    public class PersonAdapter : IPersonAdapter
    {
        private ILogger _logger;
        private readonly IFaceAdapter _faceAdapter;
        private readonly IAnalyzeRequestRepository _analyzeRequestRepository;

        public PersonAdapter(ILogger<PersonAdapter> logger,
                             IFaceAdapter faceAdapter,
                             IAnalyzeRequestRepository analyzeRequestRepository)
        {
            _logger = logger;
            _faceAdapter = faceAdapter;
            _analyzeRequestRepository = analyzeRequestRepository;
        }

        public async Task<bool> CreatePersonGroupAsync(PersonGroup personGroup)
        {
            var connectionApiData = await GetConnectionApiData("CreatePersonGroupAsync", RequestType.PersonGroupCreate.ToString());
            if (connectionApiData == null) return false;

            await _faceAdapter.CreatePersonGroupAsync(personGroup.IdApi.ToString(), personGroup.Name, connectionApiData);
            return true;
        }

        public async Task<string> CreatePersonAsync(Person person)
        {
            var connectionApiData = await GetConnectionApiData("CreatePersonAsync", RequestType.PersonGroupPersonCreate.ToString());
            if (connectionApiData == null) return null;

            return await _faceAdapter.CreatePersonGroupPersonAsync(person.PersonGroup.IdApi.ToString(), person.Name, connectionApiData);
        }

        public async Task<string> AddPersonFaceAsync(Person person, byte[] image)
        {
            var connectionApiData = await GetConnectionApiData("AddPersonFaceAsync", RequestType.PersonGroupPersonAddFace.ToString());
            if (connectionApiData == null) return null;

            return await _faceAdapter.AddFacePersonGroupPersonAsync(person.PersonGroup.IdApi.ToString(), person.IdApi.ToString(), image, connectionApiData);
        }

        public async Task<bool> TrainPersonGroupAsync(PersonGroup personGroup)
        {
            var connectionApiData = await GetConnectionApiData("TrainPersonGroupAsync", RequestType.PersonGroupTrain.ToString());
            if (connectionApiData == null) return false;

            await _faceAdapter.TrainPersonGroupAsync(personGroup.IdApi.ToString(), connectionApiData);
            return true;
        }

        public async Task<Status> GetTrainingStatusPersonGroupAsync(PersonGroup personGroup)
        {
            var connectionApiData = await GetConnectionApiData("GetTrainingStatusPersonGroupAsync", RequestType.PersonGroupTrainGetStatus.ToString());
            if (connectionApiData == null) return Status.Error;

            var statusString = await _faceAdapter.GetTrainingStatusPersonGroupAsync(personGroup.IdApi.ToString(), connectionApiData);
            return (Status)Enum.Parse(typeof(Status), statusString);
        }

        private async Task<ConnectionApiData> GetConnectionApiData(string callMethod, string typeName)
        {
            var analyseRequest = await _analyzeRequestRepository.GetByTypeNameAsync(typeName);
            if (analyseRequest == null)
            {
                _logger.LogError($"PersonAdapter.GetConnectionApiData.{callMethod}::Cannot get AnalyseRequest for TypeName:{typeName}");
                return null;
            }

            return new ConnectionApiData
            {
                Location = analyseRequest.Location,
                SubscriptionKey = analyseRequest.SubscriptionKey
            };
        }
    }
}
