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
    public class GivenRepository : Repository<Given>, IGivenRepository
    {
        public GivenRepository(IOptions<DbConnection> dbConnection)
            : base("DonationsGivens", dbConnection.Value.DefaultConnection)
        {
        }

        public override async Task<Given> GetByIdAsync(int id)
        {
            using (IDbConnection dbConnection = GetConnection())
            {
                var parameters = new DynamicParameters();
                parameters.Add("@Id", id, DbType.Int64, ParameterDirection.Input);

                dbConnection.Open();
                var query = await dbConnection.QueryAsync<Given, Donation, User, Location, Given>(
                    "USP_SEL_DonationsGivenById",
                    map: (given, donation, user, location) =>
                    {
                        given.Donation = donation;
                        given.User = user;
                        given.Location = location;
                        return given;
                    },
                    param: parameters,
                    commandType: CommandType.StoredProcedure
                );

                return await query.ToAsyncEnumerable().Distinct().FirstOrDefault();
            }
        }

        public async Task<Given> GetByIdSystemLanguageAsync(int id, int idSystems, int idLanguage)
        {
            using (IDbConnection dbConnection = GetConnection())
            {
                var parameters = new DynamicParameters();
                parameters.Add("@Id", id, DbType.Int64, ParameterDirection.Input);
                parameters.Add("@IdSystems", idSystems, DbType.Int64, ParameterDirection.Input);
                parameters.Add("@IdLanguages", idLanguage, DbType.Int64, ParameterDirection.Input);

                dbConnection.Open();
                var query = await dbConnection.QueryAsync<Given, Donation, DonationName, User, Location, Given>(
                    "USP_SEL_DonationsGivenByIdSystemLanguage",
                    map: (given, donation, donationName, user, location) =>
                    {
                        donation.AddName(donationName);
                        given.Donation = donation;
                        given.User = user;
                        given.Location = location;
                        return given;
                    },
                    param: parameters,
                    commandType: CommandType.StoredProcedure
                );

                return await query.ToAsyncEnumerable().Distinct().FirstOrDefault();
            }
        }

        public async Task<IEnumerable<Given>> ListByOwnerAsync(int idUserOwner, int idSystems, int idLanguage)
        {
            using (IDbConnection dbConnection = GetConnection())
            {
                var parameters = new DynamicParameters();
                parameters.Add("@IdUserOwner", idUserOwner, DbType.Int64, ParameterDirection.Input);
                parameters.Add("@IdSystems", idSystems, DbType.Int64, ParameterDirection.Input);
                parameters.Add("@IdLanguages", idLanguage, DbType.Int64, ParameterDirection.Input);

                dbConnection.Open();

                var desiredDictionary = new Dictionary<int, Given>();
                var query = await dbConnection.QueryAsync<Given, Donation, DonationName, User, Location, Given>(
                    "USP_SEL_DonationsGivenByOwner",
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

        public async Task AddAsync(Given obj)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@IdDonations", obj.Donation.Id, DbType.Int64, ParameterDirection.Input);
            parameters.Add("@IdUserOwner", obj.User.Id, DbType.Int64, ParameterDirection.Input);
            parameters.Add("@DtExp", obj.DtExp, DbType.DateTime, ParameterDirection.Input);
            parameters.Add("@Img", obj.Img, DbType.Binary, ParameterDirection.Input);
            parameters.Add("@Address", obj.Location.Address, DbType.String, ParameterDirection.Input);
            parameters.Add("@City", obj.Location.City, DbType.String, ParameterDirection.Input);
            parameters.Add("@State", obj.Location.State, DbType.String, ParameterDirection.Input);
            parameters.Add("@ZipCode", obj.Location.ZipCode, DbType.String, ParameterDirection.Input);
            parameters.Add("@Latitude", obj.Location.Latitude, DbType.Double, ParameterDirection.Input);
            parameters.Add("@Longitude", obj.Location.Longitude, DbType.Double, ParameterDirection.Input);
            parameters.Add("@MaxLetinMeters", obj.MaxLetinMeters, DbType.Double, ParameterDirection.Input);
            await base.AddAsync("USP_INS_DonationsGiven", parameters, CommandType.StoredProcedure);
        }

        public async Task UpdateAsync(Given obj)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@Id", obj.Id, DbType.Int64, ParameterDirection.Input);
            parameters.Add("@IdDonations", obj.Donation.Id, DbType.Int64, ParameterDirection.Input);
            parameters.Add("@IdUserOwner", obj.User.Id, DbType.Int64, ParameterDirection.Input);
            parameters.Add("@DtExp", obj.DtExp, DbType.DateTime, ParameterDirection.Input);
            parameters.Add("@Img", obj.Img, DbType.Binary, ParameterDirection.Input);
            parameters.Add("@Address", obj.Location.Address, DbType.String, ParameterDirection.Input);
            parameters.Add("@City", obj.Location.City, DbType.String, ParameterDirection.Input);
            parameters.Add("@State", obj.Location.State, DbType.String, ParameterDirection.Input);
            parameters.Add("@ZipCode", obj.Location.ZipCode, DbType.String, ParameterDirection.Input);
            parameters.Add("@Latitude", obj.Location.Latitude, DbType.Double, ParameterDirection.Input);
            parameters.Add("@Longitude", obj.Location.Longitude, DbType.Double, ParameterDirection.Input);
            parameters.Add("@MaxLetinMeters", obj.MaxLetinMeters, DbType.Double, ParameterDirection.Input);
            parameters.Add("@Active", obj.Active, DbType.Int16, ParameterDirection.Input);
            await base.UpdateAsync("USP_UPD_DonationsGiven", parameters, CommandType.StoredProcedure);
        }

        public async Task AddImageAsync(int idDonationsGivens, byte[] image)
        {
            using (IDbConnection dbConnection = GetConnection())
            {
                var parameters = new DynamicParameters();
                parameters.Add("@Id", idDonationsGivens, DbType.Int64, ParameterDirection.Input);
                parameters.Add("@Img", image, DbType.Binary, ParameterDirection.Input);

                dbConnection.Open();
                await dbConnection.ExecuteAsync("USP_UPD_DonationsGivenImg", parameters, commandType:CommandType.StoredProcedure);
            }
        }
    }
}
