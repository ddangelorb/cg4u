using System;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using CG4U.Core.Common.Domain.Models;
using CG4U.Security.Domain.Configuration;
using CG4U.Security.Domain.Configuration.Repository;
using Dapper;
using Microsoft.Extensions.Options;

namespace CG4U.Security.Infra.Data.Repository
{
    public class VideoCameraRepository : Repository<VideoCamera>, IVideoCameraRepository
    {
        public VideoCameraRepository(IOptions<DbConnection> dbConnection)
            : base("VideoCameras", dbConnection.Value.DefaultConnection)
        {
        }

        public override async Task<VideoCamera> GetByIdAsync(int id)
        {
            using (IDbConnection dbConnection = GetConnection())
            {
                var parameters = new DynamicParameters();
                parameters.Add("@Id", id, DbType.Int64, ParameterDirection.Input);

                dbConnection.Open();
                var query = await dbConnection.QueryAsync<VideoCamera, AnalyzeRequest, VideoCamera>(
                    "USP_SEL_VideoCamerasById",
                    map: (videoCamera, analyzeRequest) =>
                    {
                        videoCamera.AddAnalyzesRequest(analyzeRequest);
                        return videoCamera;
                    },
                    param: parameters,
                    commandType: CommandType.StoredProcedure
                );

                return await query.ToAsyncEnumerable().Distinct().FirstOrDefault();
            }
        }

        public async Task<VideoCamera> GetByIdPersonGroupAndAnalyzeCodeAsync(int idPersonGroup, int analyzeCode)
        {
            using (IDbConnection dbConnection = GetConnection())
            {
                var parameters = new DynamicParameters();
                parameters.Add("@IdPersonGroups", idPersonGroup, DbType.Int64, ParameterDirection.Input);
                parameters.Add("@TypeCode", analyzeCode, DbType.Int64, ParameterDirection.Input);

                dbConnection.Open();
                var query = await dbConnection.QueryAsync<VideoCamera, AnalyzeRequest, VideoCamera>(
                    "USP_SEL_VideoCamerasByIdPersonGroupAndAnalyzeCode",
                    map: (videoCamera, analyzeRequest) =>
                    {
                        videoCamera.AddAnalyzesRequest(analyzeRequest);
                        return videoCamera;
                    },
                    param: parameters,
                    commandType: CommandType.StoredProcedure
                );

                return await query.ToAsyncEnumerable().Distinct().FirstOrDefault();
            }
        }

        public async Task AddAsync(VideoCamera obj)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@IdPersonGroups", obj.IdPersonGroups, DbType.Int64, ParameterDirection.Input);
            parameters.Add("@IdPersonGroupsAPI", obj.IdPersonGroupsAPI, DbType.String, ParameterDirection.Input);
            parameters.Add("@Name", obj.Name, DbType.String, ParameterDirection.Input);
            parameters.Add("@Description", obj.Description, DbType.String, ParameterDirection.Input);
            await base.AddAsync("USP_INS_VideoCameras", parameters, CommandType.StoredProcedure);
        }

        public Task UpdateAsync(VideoCamera obj)
        {
            throw new NotImplementedException();
        }
    }
}
