using System.Collections.Generic;
using System.Threading.Tasks;
using CG4U.Donate.ClientApp.Med.Services.Root;

namespace CG4U.Donate.ClientApp.Med.Services.Interfaces
{
    public interface ITradeService
    {
        Task<List<RootDesired>> ListMatchDesiredsByPositionAsync(int idDonationsGivens, double maxDistanceValue);
        Task<List<RootGiven>> ListMatchGivensByPositionAsync(int idDonationsDesired, double maxDistanceValue);
    }
}
