using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using CG4U.Core.Common.Domain.Models;
using CG4U.Donate.Domain.Common;
using CG4U.Donate.Domain.Donations;
using CG4U.Donate.Domain.Donations.Repository;
using Dapper;
using Microsoft.Extensions.Options;

namespace CG4U.Donate.Infra.Data.Repository
{
    public class DesiredRepository : Repository<Desired>, IDesiredRepository
    {
        public DesiredRepository(IOptions<DbConnection> dbConnection)
            : base("DonationsDesired", dbConnection.Value.DefaultConnection)
        {
        }

        public override async Task<Desired> GetByIdAsync(int id)
        {
            using (IDbConnection dbConnection = GetConnection())
            {
                var parameters = new DynamicParameters();
                parameters.Add("@Id", id, DbType.Int64, ParameterDirection.Input);

                dbConnection.Open();
                var query = await dbConnection.QueryAsync<Desired, Donation, User, Location, Desired>(
                    "USP_SEL_DonationsDesiredById",
                    map: (desired, donation, user, location) =>
                    {
                        desired.Donation = donation;
                        desired.User = user;
                        desired.Location = location;
                        return desired;
                    },
                    param: parameters,
                    commandType: CommandType.StoredProcedure
                );

                return await query.ToAsyncEnumerable().Distinct().FirstOrDefault();
            }
        }

        public async Task<Desired> GetByIdSystemLanguageAsync(int id, int idSystems, int idLanguage)
        {
            using (IDbConnection dbConnection = GetConnection())
            {
                var parameters = new DynamicParameters();
                parameters.Add("@Id", id, DbType.Int64, ParameterDirection.Input);
                parameters.Add("@IdSystems", idSystems, DbType.Int64, ParameterDirection.Input);
                parameters.Add("@IdLanguages", idLanguage, DbType.Int64, ParameterDirection.Input);

                dbConnection.Open();
                var query = await dbConnection.QueryAsync<Desired, Donation, DonationName, User, Location, Desired>(
                    "USP_SEL_DonationsDesiredByIdSystemLanguage",
                    map: (desired, donation, donationName, user, location) =>
                    {
                        donation.AddName(donationName);
                        desired.Donation = donation;
                        desired.User = user;
                        desired.Location = location;
                        return desired;
                    },
                    param: parameters,
                    commandType: CommandType.StoredProcedure
                );

                return await query.ToAsyncEnumerable().Distinct().FirstOrDefault();
            }
        }

        public async Task<IEnumerable<Desired>> ListByOwnerAsync(int idUserOwner, int idSystems, int idLanguage)
        {
            using (IDbConnection dbConnection = GetConnection())
            {
                var parameters = new DynamicParameters();
                parameters.Add("@IdUserOwner", idUserOwner, DbType.Int64, ParameterDirection.Input);
                parameters.Add("@IdSystems", idSystems, DbType.Int64, ParameterDirection.Input);
                parameters.Add("@IdLanguages", idLanguage, DbType.Int64, ParameterDirection.Input);

                dbConnection.Open();

                var desiredDictionary = new Dictionary<int, Desired>();
                var query = await dbConnection.QueryAsync<Desired, Donation, DonationName, User, Location, Desired>(
                    "USP_SEL_DonationsDesiredByOwner",
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

        public async Task AddAsync(Desired obj)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@IdDonations", obj.Donation.Id, DbType.Int64, ParameterDirection.Input);
            parameters.Add("@IdUserOwner", obj.User.Id, DbType.Int64, ParameterDirection.Input);
            parameters.Add("@DtExp", obj.DtExp, DbType.DateTime, ParameterDirection.Input);
            parameters.Add("@Address", obj.Location.Address, DbType.String, ParameterDirection.Input);
            parameters.Add("@City", obj.Location.City, DbType.String, ParameterDirection.Input);
            parameters.Add("@State", obj.Location.State, DbType.String, ParameterDirection.Input);
            parameters.Add("@ZipCode", obj.Location.ZipCode, DbType.String, ParameterDirection.Input);
            parameters.Add("@Latitude", obj.Location.Latitude, DbType.Double, ParameterDirection.Input);
            parameters.Add("@Longitude", obj.Location.Longitude, DbType.Double, ParameterDirection.Input);
            parameters.Add("@MaxGetinMeters", obj.MaxGetinMeters, DbType.Double, ParameterDirection.Input);
            await base.AddAsync("USP_INS_DonationsDesired", parameters, CommandType.StoredProcedure);
        }

        public async Task UpdateAsync(Desired obj)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@Id", obj.Id, DbType.Int64, ParameterDirection.Input);
            parameters.Add("@IdDonations", obj.Donation.Id, DbType.Int64, ParameterDirection.Input);
            parameters.Add("@IdUserOwner", obj.User.Id, DbType.Int64, ParameterDirection.Input);
            parameters.Add("@DtExp", obj.DtExp, DbType.DateTime, ParameterDirection.Input);
            parameters.Add("@Address", obj.Location.Address, DbType.String, ParameterDirection.Input);
            parameters.Add("@City", obj.Location.City, DbType.String, ParameterDirection.Input);
            parameters.Add("@State", obj.Location.State, DbType.String, ParameterDirection.Input);
            parameters.Add("@ZipCode", obj.Location.ZipCode, DbType.String, ParameterDirection.Input);
            parameters.Add("@Latitude", obj.Location.Latitude, DbType.Double, ParameterDirection.Input);
            parameters.Add("@Longitude", obj.Location.Longitude, DbType.Double, ParameterDirection.Input);
            parameters.Add("@MaxGetinMeters", obj.MaxGetinMeters, DbType.Double, ParameterDirection.Input);
            parameters.Add("@Active", obj.Active, DbType.Int16, ParameterDirection.Input);
            await base.UpdateAsync("USP_UPD_DonationsDesired", parameters, CommandType.StoredProcedure);
        }
    }
}
