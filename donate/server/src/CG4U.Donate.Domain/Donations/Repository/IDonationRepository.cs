using System.Collections.Generic;
using System.Threading.Tasks;
using CG4U.Core.Common.Domain.Interfaces;

namespace CG4U.Donate.Domain.Donations.Repository
{
    public interface IDonationRepository : IRepository<Donation>
    {
        Task<Donation> GetByIdSystemLanguageAsync(int id, int idSystems, int idLanguage);
        Task<IEnumerable<Donation>> GetByLanguageAndNameAsync(int idSystems, int idLanguage, string name);
    }
}
