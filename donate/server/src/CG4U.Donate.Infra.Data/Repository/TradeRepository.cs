using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using CG4U.Core.Common.Domain.Models;
using CG4U.Donate.Domain.Common;
using CG4U.Donate.Domain.Donations;
using CG4U.Donate.Domain.Trades;
using CG4U.Donate.Domain.Trades.Repository;
using Dapper;
using Microsoft.Extensions.Options;

namespace CG4U.Donate.Infra.Data.Repository
{
    public class TradeRepository : Repository<Trade>, ITradeRepository
    {
        public TradeRepository(IOptions<DbConnection> dbConnection)
            : base("Trades", dbConnection.Value.DefaultConnection)
        {
        }

        public override async Task<Trade> GetByIdAsync(int id)
        {
            using (IDbConnection dbConnection = GetConnection())
            {
                var parameters = new DynamicParameters();
                parameters.Add("@Id", id, DbType.Int64, ParameterDirection.Input);

                dbConnection.Open();
                var query = await dbConnection.QueryAsync<Trade, User, User, Given, Desired, Trade>(
                    "USP_SEL_TradesById",
                    map: (trade, userGet, userLet, given, desired) =>
                    {
                        trade.UserGet = userGet;
                        trade.UserLet = userLet;
                        trade.Given = given;
                        trade.Desired = desired;
                        return trade;
                    },
                    param: parameters,
                    commandType: CommandType.StoredProcedure
                );

                return await query.ToAsyncEnumerable().Distinct().FirstOrDefault();
            }
        }

        public async Task<Trade> GetByIdSystemLanguageAsync(int id, int idSystems, int idLanguage)
        {
            using (IDbConnection dbConnection = GetConnection())
            {
                var parameters = new DynamicParameters();
                parameters.Add("@Id", id, DbType.Int64, ParameterDirection.Input);
                parameters.Add("@IdSystems", idSystems, DbType.Int64, ParameterDirection.Input);
                parameters.Add("@IdLanguages", idLanguage, DbType.Int64, ParameterDirection.Input);

                dbConnection.Open();
                var query = await dbConnection.QueryAsync<Trade, User, User, Given, Desired, DonationName, Trade>(
                    "USP_SEL_TradesByIdSystemLanguage",
                    map: (trade, userGet, userLet, given, desired, donationName) =>
                    {
                        trade.UserGet = userGet;
                        trade.UserLet = userLet;

                        var donation = new Donation()
                        {
                            Id = donationName.IdDonations,
                            IdDonations = donationName.IdDonations,
                            IdSystems = idSystems
                        };
                        donation.AddName(donationName);
                        given.Donation = donation;
                        desired.Donation = donation;

                        trade.Given = given;
                        trade.Desired = desired;

                        return trade;
                    },
                    param: parameters,
                    commandType: CommandType.StoredProcedure
                );

                return await query.ToAsyncEnumerable().Distinct().FirstOrDefault();
            }
        }

        public async Task<IEnumerable<Desired>> ListMatchDesiredsByPositionAsync(int idDonationsGivens, int idSystems, int idLanguages, double maxDistanceInMeters)
        {
            using (IDbConnection dbConnection = GetConnection())
            {
                var parameters = new DynamicParameters();
                parameters.Add("@IdDonationsGivens", idDonationsGivens, DbType.Int64, ParameterDirection.Input);
                parameters.Add("@IdSystems", idSystems, DbType.Int64, ParameterDirection.Input);
                parameters.Add("@IdLanguages", idLanguages, DbType.Int64, ParameterDirection.Input);
                parameters.Add("@MaxDistanceInMeters", maxDistanceInMeters, DbType.Double, ParameterDirection.Input);

                dbConnection.Open();

                var desiredDictionary = new Dictionary<int, Desired>();
                var query = await dbConnection.QueryAsync<Desired, Donation, DonationName, User, Location, Desired>(
                    "USP_SEL_TradeMatchDesireds",
                    map: (desired, donation, donationName, user, location) =>
                    {
                        Desired desiredEntry;

                        if (!desiredDictionary.TryGetValue(desired.Id, out desiredEntry))
                        {
                            desired.Donation = donation;
                            desired.User = user;
                            desired.Location = location;

                            desiredEntry = desired;
                            desiredDictionary.Add(desiredEntry.Id, desiredEntry);
                        }

                        desiredEntry.Donation.AddName(donationName);
                        return desiredEntry;
                    },
                    param: parameters,
                    commandType: CommandType.StoredProcedure
                );

                return query;
            }
        }

        public async Task<IEnumerable<Given>> ListMatchGivensByPositionAsync(int idDonationsDesired, int idSystems, int idLanguages, double maxDistanceInMeters)
        {
            using (IDbConnection dbConnection = GetConnection())
            {
                var parameters = new DynamicParameters();
                parameters.Add("@IdDonationsDesired", idDonationsDesired, DbType.Int64, ParameterDirection.Input);
                parameters.Add("@IdSystems", idSystems, DbType.Int64, ParameterDirection.Input);
                parameters.Add("@IdLanguages", idLanguages, DbType.Int64, ParameterDirection.Input);
                parameters.Add("@MaxDistanceInMeters", maxDistanceInMeters, DbType.Double, ParameterDirection.Input);

                dbConnection.Open();

                var desiredDictionary = new Dictionary<int, Given>();
                var query = await dbConnection.QueryAsync<Given, Donation, DonationName, User, Location, Given>(
                    "USP_SEL_TradeMatchGivens",
                    map: (given, donation, donationName, user, location) =>
                    {
                        Given givenEntry;

                        if (!desiredDictionary.TryGetValue(given.Id, out givenEntry))
                        {
                            given.Donation = donation;
                            given.User = user;
                            given.Location = location;

                            givenEntry = given;
                            desiredDictionary.Add(givenEntry.Id, givenEntry);
                        }

                        givenEntry.Donation.AddName(donationName);
                        return givenEntry;
                    },
                    param: parameters,
                    commandType: CommandType.StoredProcedure
                );

                return query;
            }
        }

        public async Task<Location> GetLocationByIdAsync(int idTradeLocations)
        {
            using (IDbConnection dbConnection = GetConnection())
            {
                var parameters = new DynamicParameters();
                parameters.Add("@idTradeLocations", idTradeLocations, DbType.Int64, ParameterDirection.Input);

                dbConnection.Open();
                return await dbConnection.QueryFirstOrDefaultAsync<Location>("SELECT *, Id AS IdLocation, IdTrades AS IdParent FROM TradeLocations WHERE Id=@idTradeLocations AND Active=1", parameters);
            }
        }

        public async Task<Evaluation> GetEvaluationByIdAsync(int idTradeEvaluations)
        {
            using (IDbConnection dbConnection = GetConnection())
            {
                var parameters = new DynamicParameters();
                parameters.Add("@idTradeEvaluations", idTradeEvaluations, DbType.Int64, ParameterDirection.Input);

                dbConnection.Open();
                return await dbConnection.QueryFirstOrDefaultAsync<Evaluation>("SELECT *, Id AS IdEvaluation, IdTrades AS IdParent FROM TradeEvaluations WHERE Id=@idTradeEvaluations AND Active=1", parameters);
            }
        }

        public async Task AddAsync(Trade obj)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@IdUserGet", obj.UserGet.Id, DbType.Int64, ParameterDirection.Input);
            parameters.Add("@IdUserLet", obj.UserLet.Id, DbType.Int64, ParameterDirection.Input);
            parameters.Add("@IdDonationsGivens", obj.Given.Id, DbType.Int64, ParameterDirection.Input);
            parameters.Add("@IdDonationsDesired", obj.Desired.Id, DbType.Int64, ParameterDirection.Input);
            parameters.Add("@DtTrade", obj.DtTrade, DbType.DateTime, ParameterDirection.Input);
            parameters.Add("@Commited", obj.Commited, DbType.Int64, ParameterDirection.Input);
            await base.AddAsync("USP_INS_Trades", parameters, CommandType.StoredProcedure);
        }

        public async Task AddLocationAsync(int idTrades, Location location)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@IdTrades", idTrades, DbType.Int64, ParameterDirection.Input);
            parameters.Add("@Address", location.Address, DbType.String, ParameterDirection.Input);
            parameters.Add("@City", location.City, DbType.String, ParameterDirection.Input);
            parameters.Add("@State", location.State, DbType.String, ParameterDirection.Input);
            parameters.Add("@ZipCode", location.ZipCode, DbType.String, ParameterDirection.Input);
            parameters.Add("@Latitude", location.Latitude, DbType.Double, ParameterDirection.Input);
            parameters.Add("@Longitude", location.Longitude, DbType.Double, ParameterDirection.Input);
            await base.AddAsync("USP_INS_TradeLocations", parameters, CommandType.StoredProcedure);
        }

        public async Task AddEvaluationAsync(int idTrades, Evaluation evaluation)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@IdTrades", idTrades, DbType.Int64, ParameterDirection.Input);
            parameters.Add("@UserGetGrade", evaluation.UserGetGrade, DbType.Int32, ParameterDirection.Input);
            parameters.Add("@UserLetGrade", evaluation.UserLetGrade, DbType.Int32, ParameterDirection.Input);
            parameters.Add("@CommentsUserGet", evaluation.CommentsUserGet, DbType.String, ParameterDirection.Input);
            parameters.Add("@CommentsUserLet", evaluation.CommentsUserLet, DbType.String, ParameterDirection.Input);
            parameters.Add("@DtEvaluationGet", evaluation.DtEvaluationGet, DbType.DateTime, ParameterDirection.Input);
            parameters.Add("@DtEvaluationLet", evaluation.DtEvaluationLet, DbType.DateTime, ParameterDirection.Input);
            parameters.Add("@ActiveGet", evaluation.ActiveGet, DbType.Int32, ParameterDirection.Input);
            parameters.Add("@ActiveLet", evaluation.ActiveLet, DbType.Int32, ParameterDirection.Input);
            await base.AddAsync("USP_INS_TradeEvaluations", parameters, CommandType.StoredProcedure);
        }

        public async Task DisableLocationAsync(int idTrades, int idTradeLocations)
        {
            using (IDbConnection dbConnection = GetConnection())
            {
                var parameters = new DynamicParameters();
                parameters.Add("@id", idTradeLocations, DbType.Int64, ParameterDirection.Input);
                parameters.Add("@idTrades", idTrades, DbType.Int64, ParameterDirection.Input);

                dbConnection.Open();
                await dbConnection.ExecuteAsync("UPDATE TradeLocations SET Active=0 WHERE Id=@id AND IdTrades=@idTrades", parameters);
            }
        }

        public async Task DisableEvaluationAsync(int idTrades, int idTradeEvaluations)
        {
            using (IDbConnection dbConnection = GetConnection())
            {
                var parameters = new DynamicParameters();
                parameters.Add("@id", idTradeEvaluations, DbType.Int64, ParameterDirection.Input);
                parameters.Add("@idTrades", idTrades, DbType.Int64, ParameterDirection.Input);

                dbConnection.Open();
                await dbConnection.ExecuteAsync("UPDATE TradeEvaluations SET Active=0 WHERE Id=@id AND IdTrades=@idTrades", parameters);
            }
        }

        public async Task<IEnumerable<Trade>> ListByUserGetSystemLanguageAsync(int idUserGet, int idSystems, int idLanguages)
        {
            return await ListByIdUserSystemLanguageAsync(idUserGet, idSystems, idLanguages, "@IdUserGet", "USP_SEL_TradesByUserGetSystemLanguage");
        }

        public async Task<IEnumerable<Trade>> ListByUserLetSystemLanguageAsync(int idUserLet, int idSystems, int idLanguages)
        {
            return await ListByIdUserSystemLanguageAsync(idUserLet, idSystems, idLanguages, "@IdUserLet", "USP_SEL_TradesByUserLetSystemLanguage");
        }

        public async Task<IEnumerable<Location>> ListTradeLocationsByIdTradesAsync(int idTrades)
        {
            using (IDbConnection dbConnection = GetConnection())
            {
                var parameters = new DynamicParameters();
                parameters.Add("@IdTrades", idTrades, DbType.Int64, ParameterDirection.Input);

                dbConnection.Open();
                return await dbConnection.QueryAsync<Location>("SELECT *, Id AS IdLocation, IdTrades AS IdParent FROM TradeLocations WHERE IdTrades=@IdTrades AND Active=1", parameters);
            }
        }

        public async Task<IEnumerable<Evaluation>> ListTradeEvaluationsByIdTradesAsync(int idTrades)
        {
            using (IDbConnection dbConnection = GetConnection())
            {
                var parameters = new DynamicParameters();
                parameters.Add("@IdTrades", idTrades, DbType.Int64, ParameterDirection.Input);

                dbConnection.Open();
                return await dbConnection.QueryAsync<Evaluation>("SELECT *, Id AS IdEvaluation, IdTrades AS IdParent FROM TradeEvaluations WHERE IdTrades=@IdTrades", parameters);
            }
        }

        public async Task UpdateAsync(Trade obj)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@Id", obj.Id, DbType.Int64, ParameterDirection.Input);
            parameters.Add("@IdUserGet", obj.UserGet.Id, DbType.Int64, ParameterDirection.Input);
            parameters.Add("@IdUserLet", obj.UserLet.Id, DbType.Int64, ParameterDirection.Input);
            parameters.Add("@IdDonationsGivens", obj.Given.Id, DbType.Int64, ParameterDirection.Input);
            parameters.Add("@IdDonationsDesired", obj.Desired.Id, DbType.Int64, ParameterDirection.Input);
            parameters.Add("@DtTrade", obj.DtTrade, DbType.DateTime, ParameterDirection.Input);
            parameters.Add("@Commited", obj.Commited, DbType.Int64, ParameterDirection.Input);
            parameters.Add("@Active", obj.Active, DbType.Int16, ParameterDirection.Input);
            await base.UpdateAsync("USP_UPD_Trades", parameters, CommandType.StoredProcedure);
        }

        public async Task UpdateLocationAsync(int idTrades, Location location)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@Id", location.Id, DbType.Int64, ParameterDirection.Input);
            parameters.Add("@IdTrades", idTrades, DbType.Int64, ParameterDirection.Input);
            parameters.Add("@Address", location.Address, DbType.String, ParameterDirection.Input);
            parameters.Add("@City", location.City, DbType.String, ParameterDirection.Input);
            parameters.Add("@State", location.State, DbType.String, ParameterDirection.Input);
            parameters.Add("@ZipCode", location.ZipCode, DbType.String, ParameterDirection.Input);
            parameters.Add("@Latitude", location.Latitude, DbType.Double, ParameterDirection.Input);
            parameters.Add("@Longitude", location.Longitude, DbType.Double, ParameterDirection.Input);
            parameters.Add("@Active", location.Active, DbType.Int16, ParameterDirection.Input);
            await base.UpdateAsync("USP_UPD_TradeLocations", parameters, CommandType.StoredProcedure);
        }

        public async Task UpdateEvaluationAsync(int idTrades, Evaluation evaluation)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@Id", evaluation.Id, DbType.Int64, ParameterDirection.Input);
            parameters.Add("@IdTrades", idTrades, DbType.Int64, ParameterDirection.Input);
            parameters.Add("@UserGetGrade", evaluation.UserGetGrade, DbType.Int32, ParameterDirection.Input);
            parameters.Add("@UserLetGrade", evaluation.UserLetGrade, DbType.Int32, ParameterDirection.Input);
            parameters.Add("@CommentsUserGet", evaluation.CommentsUserGet, DbType.String, ParameterDirection.Input);
            parameters.Add("@CommentsUserLet", evaluation.CommentsUserLet, DbType.String, ParameterDirection.Input);
            parameters.Add("@DtEvaluationGet", evaluation.DtEvaluationGet, DbType.DateTime, ParameterDirection.Input);
            parameters.Add("@DtEvaluationLet", evaluation.DtEvaluationLet, DbType.DateTime, ParameterDirection.Input);
            parameters.Add("@ActiveGet", evaluation.ActiveGet, DbType.Int32, ParameterDirection.Input);
            parameters.Add("@ActiveLet", evaluation.ActiveLet, DbType.Int32, ParameterDirection.Input);
            await base.UpdateAsync("USP_UPD_TradeEvaluations", parameters, CommandType.StoredProcedure);
        }

        private async Task<IEnumerable<Trade>> ListByIdUserSystemLanguageAsync(int idUser, int idSystems, int idLanguages, string paramIdName, string uspName)
        {
            using (IDbConnection dbConnection = GetConnection())
            {
                var parameters = new DynamicParameters();
                parameters.Add(paramIdName, idUser, DbType.Int64, ParameterDirection.Input);
                parameters.Add("@IdSystems", idSystems, DbType.Int64, ParameterDirection.Input);
                parameters.Add("@IdLanguages", idLanguages, DbType.Int64, ParameterDirection.Input);

                dbConnection.Open();
                return await dbConnection.QueryAsync<Trade, User, User, Given, Desired, DonationName, Trade>(
                    uspName,
                    map: (trade, userGet, userLet, given, desired, donationName) =>
                    {
                        trade.UserGet = userGet;
                        trade.UserLet = userLet;

                        var donation = new Donation()
                        {
                            Id = donationName.IdDonations,
                            IdDonations = donationName.IdDonations,
                            IdSystems = idSystems
                        };
                        donation.AddName(donationName);
                        given.Donation = donation;
                        desired.Donation = donation;

                        trade.Given = given;
                        trade.Desired = desired;

                        return trade;
                    },
                    param: parameters,
                    commandType: CommandType.StoredProcedure
                );
            }
        }
    }
}
