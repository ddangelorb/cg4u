using System.Threading.Tasks;
using CG4U.Security.Services.Data;

namespace CG4U.Security.Services.Interfaces
{
    public interface IFaceAdapter
    {
        Task<string> IdentifyFaceAsync(byte[] image, string personGroupId, ConnectionApiData connectionApiData);
        Task CreatePersonGroupAsync(string id, string name, ConnectionApiData connectionApiData);
        Task<string> CreatePersonGroupPersonAsync(string personGroupId, string name, ConnectionApiData connectionApiData);
        Task<string> AddFacePersonGroupPersonAsync(string personGroupId, string personId, byte[] image, ConnectionApiData connectionApiData);
        Task TrainPersonGroupAsync(string personGroupId, ConnectionApiData connectionApiData);
        Task<string> GetTrainingStatusPersonGroupAsync(string personGroupId, ConnectionApiData connectionApiData);
    }
}
