using System;
using System.Data;
using System.Threading.Tasks;
using CG4U.Core.Common.Domain.Models;
using CG4U.Security.Domain.Configuration;
using CG4U.Security.Domain.Configuration.Repository;
using Dapper;
using Microsoft.Extensions.Options;

namespace CG4U.Security.Infra.Data.Repository
{
    public class AlertRepository : Repository<Alert>, IAlertRepository
    {
        public AlertRepository(IOptions<DbConnection> dbConnection)
            : base("Alerts", dbConnection.Value.DefaultConnection)
        {
        }

        public async Task AddAsync(Alert obj)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@IdAnalyzesRequests", obj.IdAnalyzesRequests, DbType.Int64, ParameterDirection.Input);
            parameters.Add("@TypeCode", obj.TypeCode, DbType.Int64, ParameterDirection.Input);
            parameters.Add("@Message", obj.Message, DbType.String, ParameterDirection.Input);
            parameters.Add("@ProcessingMethod", obj.ProcessingMethod, DbType.String, ParameterDirection.Input);
            parameters.Add("@ProcessingParam", obj.ProcessingParam, DbType.String, ParameterDirection.Input);
            await base.AddAsync("USP_INS_Alerts", parameters, CommandType.StoredProcedure);
        }

        public async Task AddConnectionPersonGroupAsync(int idPersonGroups, int idAlerts)
        {
            using (IDbConnection dbConnection = GetConnection())
            {
                var parameters = new DynamicParameters();
                parameters.Add("@IdPersonGroups", idPersonGroups, DbType.Int64, ParameterDirection.Input);
                parameters.Add("@IdAlerts", idAlerts, DbType.Int64, ParameterDirection.Input);

                dbConnection.Open();
                await dbConnection.ExecuteAsync("USP_INS_PersonGroupsAlerts", parameters, commandType: CommandType.StoredProcedure);
            }
        }

        public Task UpdateAsync(Alert obj)
        {
            throw new NotImplementedException();
        }
    }
}
