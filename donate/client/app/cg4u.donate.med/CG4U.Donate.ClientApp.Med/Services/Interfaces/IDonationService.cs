using System.Collections.Generic;
using System.Threading.Tasks;
using CG4U.Donate.ClientApp.Med.Services.Root;

namespace CG4U.Donate.ClientApp.Med.Services.Interfaces
{
    public interface IDonationService
    {
        Task<List<RootDonation>> ListDonationsByLanguageAndNameAsync(string query);
        Task<bool> AddGivenAsync(RootGiven given);
        Task<bool> AddDesiredAsync(RootDesired desired);
    }
}
