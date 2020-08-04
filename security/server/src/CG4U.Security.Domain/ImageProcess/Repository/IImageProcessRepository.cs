using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CG4U.Core.Common.Domain.Interfaces;

namespace CG4U.Security.Domain.ImageProcess.Repository
{
    public interface IImageProcessRepository : IRepository<ImageProcess>
    {
        Task<ImageProcess> GetByIdReferenceAsync(string idReference);
        Task<ImageProcessAnalyze> GetAnalyzeByIdReferenceAsync(string idAnalyzeReference);
        Task<IEnumerable<ImageProcess>> ListByReturnResponseQueryAsync(string returnResponseQuery);
        Task AddAnalyzeAsync(ImageProcessAnalyze imageProcessAnalyze);
        Task UpdateAnalyzeAsync(ImageProcessAnalyze imageProcessAnalyze);
    }
}
