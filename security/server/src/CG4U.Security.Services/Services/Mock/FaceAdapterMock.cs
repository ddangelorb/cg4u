using System;
using System.Threading.Tasks;
using CG4U.Security.Services.Data;
using CG4U.Security.Services.Interfaces;

namespace CG4U.Security.Services.Services.Mock
{
    public class FaceAdapterMock : IFaceAdapter
    {
        public async Task<string> AddFacePersonGroupPersonAsync(string personGroupId, string personId, byte[] image, ConnectionApiData connectionApiData)
        {
            return await Task.FromResult(Guid.NewGuid().ToString());
        }

        public async Task CreatePersonGroupAsync(string id, string name, ConnectionApiData connectionApiData)
        {
            await Task.FromResult(0);
        }

        public async Task<string> CreatePersonGroupPersonAsync(string personGroupId, string name, ConnectionApiData connectionApiData)
        {
            return await Task.FromResult(Guid.NewGuid().ToString());
        }

        public async Task<string> GetTrainingStatusPersonGroupAsync(string personGroupId, ConnectionApiData connectionApiData)
        {
            return await Task.FromResult(@"
                {
                    ""status"": ""succeeded"",
                    ""createdDateTime"": ""12/21/2017 12:57:27"",
                    ""lastActionDateTime"": ""12/21/2017 12:57:30"",
                    ""message"": null
                }
            ");
        }

        public async Task<string> IdentifyFaceAsync(byte[] image, string personGroupId, ConnectionApiData connectionApiData)
        {
            return await Task.FromResult(@"
                {
                    [
                        {
                            ""faceId"": ""c5c24a82-6845-4031-9d5d-978df9175426"",
                            ""candidates"": [
                                {
                                    ""personId"": ""25985303-c537-4467-b41d-bdb45cd95ca1"",
                                    ""confidence"": 0.92
                                }
                            ]
                        },
                        {
                            ""faceId"": ""65d083d4-9447-47d1-af30-b626144bf0fb"",
                            ""candidates"": [
                                {
                                    ""personId"": ""2ae4935b-9659-44c3-977f-61fac20d0538"",
                                    ""confidence"": 0.89
                                }
                            ]
                        }
                    ]
                }
            ");
        }

        public async Task TrainPersonGroupAsync(string personGroupId, ConnectionApiData connectionApiData)
        {
            await Task.FromResult(0);
        }
    }
}
