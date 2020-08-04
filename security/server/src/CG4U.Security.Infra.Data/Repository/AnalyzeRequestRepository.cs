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
    public class AnalyzeRequestRepository : Repository<AnalyzeRequest>, IAnalyzeRequestRepository
    {
        public AnalyzeRequestRepository(IOptions<DbConnection> dbConnection)
            : base("AnalyzesRequests", dbConnection.Value.DefaultConnection)
        {
        }

        public async Task AddAsync(AnalyzeRequest obj)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@IdBillableProcesses", obj.IdBillableProcesses, DbType.Int64, ParameterDirection.Input);
            parameters.Add("@IdLanguages", obj.IdLanguages, DbType.Int64, ParameterDirection.Input);
            parameters.Add("@AnalyzeOrder", obj.AnalyzeOrder, DbType.Int64, ParameterDirection.Input);
            parameters.Add("@TypeCode", obj.TypeCode, DbType.Int64, ParameterDirection.Input);
            parameters.Add("@TypeName", obj.TypeName, DbType.String, ParameterDirection.Input);
            parameters.Add("@Location", obj.Location, DbType.String, ParameterDirection.Input);
            parameters.Add("@SubscriptionKey", obj.SubscriptionKey, DbType.String, ParameterDirection.Input);
            await base.AddAsync("USP_INS_AnalyzesRequests", parameters, CommandType.StoredProcedure);
        }

        public async Task AddConnectionVideoCameraAsync(int idAnalyzesRequests, int idVideoCameras)
        {
            using (IDbConnection dbConnection = GetConnection())
            {
                var parameters = new DynamicParameters();
                parameters.Add("@IdAnalyzesRequests", idAnalyzesRequests, DbType.Int64, ParameterDirection.Input);
                parameters.Add("@IdVideoCameras", idVideoCameras, DbType.Int64, ParameterDirection.Input);

                dbConnection.Open();
                await dbConnection.ExecuteAsync("USP_INS_AnalyzesRequestsVideoCameras", parameters, commandType: CommandType.StoredProcedure);
            }
        }

        public async Task<AnalyzeRequest> GetByTypeNameAsync(string typeName)
        {
            using (IDbConnection dbConnection = GetConnection())
            {
                var parameters = new DynamicParameters();
                parameters.Add("@typeName", typeName, DbType.String, ParameterDirection.Input);

                dbConnection.Open();
                return await dbConnection.QueryFirstOrDefaultAsync<AnalyzeRequest>("SELECT * FROM AnalyzesRequests WHERE TypeName=@typeName AND Active=1", parameters);
            }
        }

        public Task UpdateAsync(AnalyzeRequest obj)
        {
            throw new NotImplementedException();
        }
    }
}
