using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using CG4U.Core.Common.Domain.Models;
using CG4U.Donate.Domain.Donations;
using CG4U.Donate.Domain.Donations.Repository;
using Dapper;
using Microsoft.Extensions.Options;

namespace CG4U.Donate.Infra.Data.Repository
{
    public class DonationRepository : Repository<Donation>, IDonationRepository
    {
        public DonationRepository(IOptions<DbConnection> dbConnection)
            : base("Donations", dbConnection.Value.DefaultConnection)
        {
        }

        public async Task<Donation> GetByIdSystemLanguageAsync(int id, int idSystems, int idLanguage)
        {
            using (IDbConnection dbConnection = GetConnection())
            {
                var parameters = new DynamicParameters();
                parameters.Add("@Id", id, DbType.Int64, ParameterDirection.Input);
                parameters.Add("@IdSystems", idSystems, DbType.Int64, ParameterDirection.Input);
                parameters.Add("@IdLanguages", idLanguage, DbType.Int64, ParameterDirection.Input);

                dbConnection.Open();
                var queryResult = await dbConnection.QueryAsync<dynamic>(
                    "USP_SEL_DonationsById", 
                    parameters,
                    commandType: CommandType.StoredProcedure);

                Slapper.AutoMapper.Configuration.AddIdentifiers(typeof(Donation), new List<string> { "IdDonations" });
                Slapper.AutoMapper.Configuration.AddIdentifiers(typeof(DonationName), new List<string> { "IdDonationsNames" });

                return (Slapper.AutoMapper.MapDynamic<Donation>(queryResult) as IEnumerable<Donation>)
                    .ToList().FirstOrDefault();
            }
        }

        public async Task<IEnumerable<Donation>> GetByLanguageAndNameAsync(int idSystems, int idLanguage, string name)
        {
            using (IDbConnection dbConnection = GetConnection())
            {
                var parameters = new DynamicParameters();
                parameters.Add("@IdSystems", idSystems  , DbType.Int64, ParameterDirection.Input);
                parameters.Add("@IdLanguages", idLanguage, DbType.Int64, ParameterDirection.Input);
                parameters.Add("@Name", RemoveAccentLetters(name), DbType.String, ParameterDirection.Input);

                dbConnection.Open();
                var queryResult = await dbConnection.QueryAsync<dynamic>(
                    "USP_SEL_DonationsSystemByLanguageAndName",
                    parameters,
                    commandType: CommandType.StoredProcedure);

                Slapper.AutoMapper.Configuration.AddIdentifiers(typeof(Donation), new List<string> { "IdDonations" });
                Slapper.AutoMapper.Configuration.AddIdentifiers(typeof(DonationName), new List<string> { "IdDonationsNames" });

                return (Slapper.AutoMapper.MapDynamic<Donation>(queryResult) as IEnumerable<Donation>)
                    .ToList();
            }
        }

        public async Task AddAsync(Donation obj)
        {
            //For now do not do anything here. Will be loaded on a static list.
            await base.AddAsync(string.Empty, null, CommandType.StoredProcedure);
        }

        public async Task UpdateAsync(Donation obj)
        {
            //For now do not do anything here. Will be loaded on a static list.
            await base.UpdateAsync(string.Empty, null, CommandType.StoredProcedure);
        }
    }
}
