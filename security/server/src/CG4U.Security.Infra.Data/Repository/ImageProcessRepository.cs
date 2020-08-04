using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using CG4U.Core.Common.Domain.Models;
using CG4U.Security.Domain.ImageProcess;
using CG4U.Security.Domain.ImageProcess.Repository;
using Dapper;
using Microsoft.Extensions.Options;

namespace CG4U.Security.Infra.Data.Repository
{
    public class ImageProcessRepository : Repository<ImageProcess>, IImageProcessRepository
    {
        public ImageProcessRepository(IOptions<DbConnection> dbConnection)
            : base("ImageProcesses", dbConnection.Value.DefaultConnection)
        {
        }

        public async Task<ImageProcess> GetByIdReferenceAsync(string idReference)
        {
            using (IDbConnection dbConnection = GetConnection())
            {
                var parameters = new DynamicParameters();
                parameters.Add("@idReference", idReference, DbType.String, ParameterDirection.Input);

                dbConnection.Open();
                return await dbConnection.QueryFirstOrDefaultAsync<ImageProcess>("SELECT * FROM ImageProcesses WHERE IdReference=@idReference AND Active=1", parameters);
            }
        }

        public async Task<ImageProcessAnalyze> GetAnalyzeByIdReferenceAsync(string idAnalyzeReference)
        {
            using (IDbConnection dbConnection = GetConnection())
            {
                var parameters = new DynamicParameters();
                parameters.Add("@idAnalyzeReference", idAnalyzeReference, DbType.String, ParameterDirection.Input);

                dbConnection.Open();
                return await dbConnection.QueryFirstOrDefaultAsync<ImageProcessAnalyze>("SELECT * FROM ImageProcessAnalyzes WHERE IdReference=@idAnalyzeReference AND Active=1", parameters);
            }
        }

        public async Task<IEnumerable<ImageProcess>> ListByReturnResponseQueryAsync(string returnResponseQuery)
        {
            using (IDbConnection dbConnection = GetConnection())
            {
                var parameters = new DynamicParameters();
                parameters.Add("@ResponseQuery", returnResponseQuery, DbType.String, ParameterDirection.Input);

                dbConnection.Open();
                return await dbConnection.QueryAsync<ImageProcess, ImageProcessAnalyze, ImageProcess>(
                    "USP_SEL_ImageProcessesByResponse",
                    map: (imageProcess, imageProcessAnalyze) =>
                    {
                        imageProcess.AddAnalyze(imageProcessAnalyze);
                        return imageProcess;
                    },
                    param: parameters,
                    commandType: CommandType.StoredProcedure
                );
            }
        }

        public async Task AddAsync(ImageProcess obj)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@IdReference", obj.IdReference, DbType.Guid , ParameterDirection.Input);
            parameters.Add("@IdVideoCameras", obj.IdVideoCameras, DbType.Int64, ParameterDirection.Input);
            parameters.Add("@ImageFile", obj.ImageFile, DbType.Binary, ParameterDirection.Input);
            parameters.Add("@ImageName", obj.ImageName, DbType.String, ParameterDirection.Input);
            parameters.Add("@IpUserRequest", obj.IpUserRequest, DbType.String, ParameterDirection.Input);
            parameters.Add("@VideoPath", obj.VideoPath, DbType.String, ParameterDirection.Input);
            parameters.Add("@SecondsToStart", obj.SecondsToStart, DbType.Int64, ParameterDirection.Input);
            parameters.Add("@DtProcess", obj.DtProcess, DbType.DateTime, ParameterDirection.Input);
            await base.AddAsync("USP_INS_ImageProcesses", parameters, CommandType.StoredProcedure);
        }

        public async Task AddAnalyzeAsync(ImageProcessAnalyze imageProcessAnalyze)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@IdReference", imageProcessAnalyze.IdReference, DbType.Guid, ParameterDirection.Input);
            parameters.Add("@IdImageProcesses", imageProcessAnalyze.IdImageProcesses, DbType.Int64, ParameterDirection.Input);
            parameters.Add("@IdAnalyzesRequestsVideoCameras", imageProcessAnalyze.IdAnalyzesRequestsVideoCameras, DbType.Int64, ParameterDirection.Input);
            parameters.Add("@DtAnalyze", imageProcessAnalyze.DtAnalyze, DbType.DateTime, ParameterDirection.Input);
            parameters.Add("@ReturnResponseType", imageProcessAnalyze.ReturnResponseType, DbType.String, ParameterDirection.Input);
            parameters.Add("@ReturnResponse", imageProcessAnalyze.ReturnResponse, DbType.String, ParameterDirection.Input);
            parameters.Add("@Commited", imageProcessAnalyze.Commited, DbType.Int64, ParameterDirection.Input);
            await base.AddAsync("USP_INS_ImageProcessAnalyzes", parameters, CommandType.StoredProcedure);
        }

        public async Task UpdateAsync(ImageProcess obj)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@Id", obj.Id, DbType.Int64, ParameterDirection.Input);
            parameters.Add("@IdReference", obj.IdReference, DbType.Guid, ParameterDirection.Input);
            parameters.Add("@IdVideoCameras", obj.IdVideoCameras, DbType.Int64, ParameterDirection.Input);
            parameters.Add("@ImageFile", obj.ImageFile, DbType.Binary, ParameterDirection.Input);
            parameters.Add("@ImageName", obj.ImageName, DbType.String, ParameterDirection.Input);
            parameters.Add("@IpUserRequest", obj.IpUserRequest, DbType.String, ParameterDirection.Input);
            parameters.Add("@VideoPath", obj.VideoPath, DbType.String, ParameterDirection.Input);
            parameters.Add("@SecondsToStart", obj.SecondsToStart, DbType.Int64, ParameterDirection.Input);
            parameters.Add("@DtProcess", obj.DtProcess, DbType.DateTime, ParameterDirection.Input);
            parameters.Add("@Active", obj.Active, DbType.Int16, ParameterDirection.Input);
            await base.UpdateAsync("USP_UPD_ImageProcesses", parameters, CommandType.StoredProcedure);
        }

        public async Task UpdateAnalyzeAsync(ImageProcessAnalyze imageProcessAnalyze)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@Id", imageProcessAnalyze.Id, DbType.Int64, ParameterDirection.Input);
            parameters.Add("@IdReference", imageProcessAnalyze.IdReference, DbType.Guid, ParameterDirection.Input);
            parameters.Add("@IdImageProcesses", imageProcessAnalyze.IdImageProcesses, DbType.Int64, ParameterDirection.Input);
            parameters.Add("@IdAnalyzesRequestsVideoCameras", imageProcessAnalyze.IdAnalyzesRequestsVideoCameras, DbType.Int64, ParameterDirection.Input);
            parameters.Add("@DtAnalyze", imageProcessAnalyze.DtAnalyze, DbType.DateTime, ParameterDirection.Input);
            parameters.Add("@ReturnResponseType", imageProcessAnalyze.ReturnResponseType, DbType.String, ParameterDirection.Input);
            parameters.Add("@ReturnResponse", imageProcessAnalyze.ReturnResponse, DbType.String, ParameterDirection.Input);
            parameters.Add("@Commited", imageProcessAnalyze.Commited, DbType.Int64, ParameterDirection.Input);
            parameters.Add("@Active", imageProcessAnalyze.Active, DbType.Int16, ParameterDirection.Input);
            await base.AddAsync("USP_UPD_ImageProcessAnalyzes", parameters, CommandType.StoredProcedure);
        }
    }
}
