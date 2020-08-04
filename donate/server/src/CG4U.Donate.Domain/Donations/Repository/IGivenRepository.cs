using System.Collections.Generic;
using System.Threading.Tasks;
using CG4U.Core.Common.Domain.Interfaces;

namespace CG4U.Donate.Domain.Donations.Repository
{
    public interface IGivenRepository : IRepository<Given>
    {
        Task<Given> GetByIdSystemLanguageAsync(int id, int idSystems, int idLanguage);
        Task<IEnumerable<Given>> ListByOwnerAsync(int idUserOwner, int idSystems, int idLanguage);
        Task AddImageAsync(int idDonationsGivens, byte[] image);
    }
}
