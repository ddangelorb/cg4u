using System.Collections.Generic;
using System.Threading.Tasks;
using CG4U.Core.Common.Domain.Interfaces;

namespace CG4U.Donate.Domain.Donations.Repository
{
    public interface IDesiredRepository : IRepository<Desired>
    {
        Task<Desired> GetByIdSystemLanguageAsync(int id, int idSystems, int idLanguage);
        Task<IEnumerable<Desired>> ListByOwnerAsync(int idUserOwner, int idSystems, int idLanguage);
    }
}
