using System.Threading.Tasks;
using CG4U.Core.Common.Domain.Interfaces;

namespace CG4U.Security.Domain.Configuration.Repository
{
    public interface IAnalyzeRequestRepository : IRepository<AnalyzeRequest>
    {
        Task<AnalyzeRequest> GetByTypeNameAsync(string typeName);
        Task AddConnectionVideoCameraAsync(int idAnalyzesRequests, int idVideoCameras);
    }
}
