using System.Collections.Generic;
using System.Threading.Tasks;
using CG4U.Security.Domain.Configuration;
using CG4U.Security.Domain.ImageProcess;
using CG4U.Security.Domain.Persons;

namespace CG4U.Security.Services.Interfaces
{
    public interface IImageProcessAdapter
    {
        Task<string> AnalyzeImageProcessAsync(VideoCamera videoCamera, ImageProcess imageProcess, AnalyzeRequest analyzeRequest);
        Task<IEnumerable<Alert>> ListAlertsByUserAndResponseAsync(int idUser, string response);
    }
}
