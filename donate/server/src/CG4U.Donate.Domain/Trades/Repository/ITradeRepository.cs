using System.Collections.Generic;
using System.Threading.Tasks;
using CG4U.Core.Common.Domain.Interfaces;
using CG4U.Donate.Domain.Common;
using CG4U.Donate.Domain.Donations;

namespace CG4U.Donate.Domain.Trades.Repository
{
    public interface ITradeRepository : IRepository<Trade>
    {
        Task<Trade> GetByIdSystemLanguageAsync(int id, int idSystems, int idLanguage);
        Task<IEnumerable<Desired>> ListMatchDesiredsByPositionAsync(int idDonationsGivens, int idSystems, int idLanguages, double maxDistanceInMeters);
        Task<IEnumerable<Given>> ListMatchGivensByPositionAsync(int idDonationsDesired, int idSystems, int idLanguages, double maxDistanceInMeters);
        Task<Location> GetLocationByIdAsync(int idTradeLocations);
        Task<Evaluation> GetEvaluationByIdAsync(int idTradeEvaluations);
        Task AddLocationAsync(int idTrades, Location location);
        Task AddEvaluationAsync(int idTrades, Evaluation evaluation);
        Task DisableLocationAsync(int idTrades, int idTradeLocations);
        Task DisableEvaluationAsync(int idTrades, int idTradeEvaluations);
        Task<IEnumerable<Trade>> ListByUserGetSystemLanguageAsync(int idUserGet, int idSystems, int idLanguages);
        Task<IEnumerable<Trade>> ListByUserLetSystemLanguageAsync(int idUserLet, int idSystems, int idLanguages);
        Task<IEnumerable<Location>> ListTradeLocationsByIdTradesAsync(int idTrades);
        Task<IEnumerable<Evaluation>> ListTradeEvaluationsByIdTradesAsync(int idTrades);
        Task UpdateLocationAsync(int idTrades, Location location);
        Task UpdateEvaluationAsync(int idTrades, Evaluation evaluation);
    }
}
