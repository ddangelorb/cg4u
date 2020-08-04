using System;
using System.Threading.Tasks;
using CG4U.Security.Domain.Configuration;
using CG4U.Security.Domain.ImageProcess;
using CG4U.Security.Services.Interfaces;
using CG4U.Security.Services.Data;
using System.Collections.Generic;
using CG4U.Security.Domain.Persons.Repository;
using Microsoft.ProjectOxford.Vision.Contract;
using Newtonsoft.Json;
using System.Text;
using Microsoft.Extensions.Logging;
using System.Text.RegularExpressions;
using Microsoft.ProjectOxford.Face.Contract;
using CG4U.Core.Common.Domain.Interfaces;

namespace CG4U.Security.Services.Services
{
    public class ImageProcessAdapter : IImageProcessAdapter
    {
        private ILogger _logger;
        private readonly IFaceAdapter _faceAdapter;
        private readonly IComputerVisionAdapter _computerVisionAdapter;
        private readonly IPersonRepository _personRepository;

        public ImageProcessAdapter(ILogger<ImageProcessAdapter> logger, 
                                   IFaceAdapter faceAdapter,
                                   IComputerVisionAdapter computerVisionAdapter,
                                   IPersonRepository personRepository)
        {
            _logger = logger;
            _faceAdapter = faceAdapter;
            _computerVisionAdapter = computerVisionAdapter;
            _personRepository = personRepository;
        }

        public async Task<string> AnalyzeImageProcessAsync(VideoCamera videoCamera, ImageProcess imageProcess, AnalyzeRequest analyzeRequest)
        {
            var connectionApiData = new ConnectionApiData
            {
                Location = analyzeRequest.Location,
                SubscriptionKey = analyzeRequest.SubscriptionKey
            };

            Languages enumLanguages = (Languages)analyzeRequest.IdLanguages;

            RequestType enumRequestTypes = (RequestType)analyzeRequest.TypeCode;
            switch (enumRequestTypes)
            {
                case RequestType.SceneChange: return string.Empty;
                case RequestType.Face: return await AnalyzeFace(imageProcess.ImageFile, videoCamera.IdPersonGroupsAPI,  connectionApiData);
                case RequestType.Carplate: return await AnalyzeCarPlate(enumLanguages.ToString(), imageProcess.ImageFile, connectionApiData);
                case RequestType.ImageDescription: return await AnalyzeImageDescription(enumLanguages.ToString(), imageProcess.ImageFile, connectionApiData);
            }

            return null;
        }

        public async Task<IEnumerable<Alert>> ListAlertsByUserAndResponseAsync(int idUser, string response)
        {
            var alertsReturn = new List<Alert>();
            var person = await _personRepository.GetByIdUserAsync(idUser);
            foreach (var alert in person.PersonGroup.Alerts)
            {
                AlertProcessingMethod enAlertProcessingMethod;
                if (Enum.TryParse(alert.ProcessingMethod, out enAlertProcessingMethod))
                {
                    switch (enAlertProcessingMethod)
                    {
                        case AlertProcessingMethod.SceneChange:
                            alertsReturn.Add(alert);
                            break;
                        case AlertProcessingMethod.UnkownCar:
                            if (alert.ProcessingParam == null)
                                _logger.LogError($"ImageProcessAdapter.ListAlertsByUserAndResponseAsync.AlertProcessingMethod.UnkownCar::Cannot find CarPlate Pattern for Alert.Id:{alert.Id}");
                            else 
                            {
                                if (await IsUnkownCarAsync(person.PersonGroup.Id, response, alert.ProcessingParam))
                                    alertsReturn.Add(alert);
                            }
                            break;
                        case AlertProcessingMethod.UnkownPeople:
                            int valueConfidence;
                            if (alert.ProcessingParam == null || !int.TryParse(alert.ProcessingParam, out valueConfidence))
                                _logger.LogError($"ImageProcessAdapter.ListAlertsByUserAndResponseAsync.AlertProcessingMethod.UnkownPeople::Cannot find integer value for MinConfidence for Alert.Id:{alert.Id}");
                            else
                            {
                                double minConfidence = valueConfidence / 100;
                                if (await IsUnkownPeopleAsync(response, minConfidence))
                                    alertsReturn.Add(alert);
                            }
                            break;
                    }
                }
            }
            return alertsReturn;
        }

        private async Task<string> AnalyzeFace(byte[] image, string personGroupId, ConnectionApiData connectionApiData)
        {
            return await _faceAdapter.IdentifyFaceAsync(image, personGroupId, connectionApiData);
        }

        private async Task<string> AnalyzeCarPlate(string language, byte[] image, ConnectionApiData connectionApiData)
        {
            return await _computerVisionAdapter.OCRAsync(language, image, connectionApiData);
        }

        private async Task<string> AnalyzeImageDescription(string language, byte[] image, ConnectionApiData connectionApiData)
        {
            return await _computerVisionAdapter.DescribeImageAsync(language, image, connectionApiData);
        }

        private async Task<bool> IsUnkownCarAsync(int idPersonGroups, string response, string plateCodePattern)
        {
            var ocrResponse = JsonConvert.DeserializeObject<OcrResults>(response);
            if (ocrResponse == null) return false;

            var plateCode = string.Empty;
            var sbPlateCodeResponse = new StringBuilder();
            foreach (var region in ocrResponse.Regions)
            {
                foreach (var line in region.Lines)
                {
                    foreach (var word in line.Words)
                    {
                        sbPlateCodeResponse.Append(word.Text);
                        if (Regex.Match(sbPlateCodeResponse.ToString(), plateCodePattern).Success)
                        {
                            plateCode = sbPlateCodeResponse.ToString();
                            break;
                        }
                    }
                }
            }

            return ! await _personRepository.IsCarBelongToPersonGroupAsync(idPersonGroups, plateCode);
        }

        private async Task<bool> IsUnkownPeopleAsync(string response, double minConfidence)
        {
            var identifyResults = JsonConvert.DeserializeObject<IdentifyResult[]>(response);
            if (identifyResults == null) return false;

            foreach (var identifyResult in identifyResults)
            {
                foreach (var candidate in identifyResult.Candidates)
                {
                    var person = await _personRepository.GetByIdApiAsync(candidate.PersonId.ToString());
                    if (person == null || candidate.Confidence < minConfidence)
                        return true;
                }
            }

            return false;
        }
    }
}
