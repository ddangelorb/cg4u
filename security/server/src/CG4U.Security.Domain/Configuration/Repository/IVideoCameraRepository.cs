using System.Threading.Tasks;
using CG4U.Core.Common.Domain.Interfaces;

namespace CG4U.Security.Domain.Configuration.Repository
{
    public interface IVideoCameraRepository : IRepository<VideoCamera>
    {
        Task<VideoCamera> GetByIdPersonGroupAndAnalyzeCodeAsync(int idPersonGroup, int analyzeCode);
    }
}
