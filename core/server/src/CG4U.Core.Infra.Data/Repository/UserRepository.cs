using System.Data;
using System.Threading.Tasks;
using CG4U.Core.Common.Domain.Models;
using CG4U.Core.Domain.Users.Repository;
using Dapper;
using Microsoft.Extensions.Options;

namespace CG4U.Core.Infra.Data.Repository
{
    public class UserRepository : Repository<User>, IUserRepository
    {
        public UserRepository(IOptions<DbConnection> dbConnection)
            : base("Users", dbConnection.Value.DefaultConnection)
        {
        }

        public async Task<User> GetByIdUserIdentityAsync(string idUserIdentity)
        {
            using (var dbConnection = GetConnection())
            {
                var parameters = new DynamicParameters();
                parameters.Add("@idUserIdentity", idUserIdentity, DbType.String, ParameterDirection.Input);

                dbConnection.Open();
                return await dbConnection.QueryFirstAsync<User>("SELECT * FROM Users WHERE IdUserIdentity=@idUserIdentity AND Active=1", parameters);
            }
        }

        public async Task<bool> IsUserHasAccessSystem(string idUserIdentity, int idSystems)
        {
            using (var dbConnection = GetConnection())
            {
                var parameters = new DynamicParameters();
                parameters.Add("@idUserIdentity", idUserIdentity, DbType.String, ParameterDirection.Input);
                parameters.Add("@idSystems", idSystems, DbType.Int64, ParameterDirection.Input);

                dbConnection.Open();
                var query = await dbConnection.QueryFirstAsync<User>("SELECT u.*, us.IdSystems FROM Users u INNER JOIN UsersSystems us ON u.Id = us.IdUsers WHERE u.IdUserIdentity=@idUserIdentity AND us.IdSystems = @idSystems AND u.Active=1 AND us.Active = 1", parameters);
                return query != null;
            }
        }

        public async Task EnableByIdUserIdentityAsync(string idUserIdentity, int idSystems)
        {
            using (var dbConnection = GetConnection())
            {
                var parameters = new DynamicParameters();
                parameters.Add("@idUserIdentity", idUserIdentity, DbType.String, ParameterDirection.Input);

                dbConnection.Open();

                await dbConnection.ExecuteAsync("UPDATE Users SET Active=1 WHERE IdUserIdentity=@idUserIdentity", parameters);

                parameters.Add("@idSystems", idUserIdentity, DbType.Int64, ParameterDirection.Input);
                await dbConnection.ExecuteAsync("UPDATE UsersSystems SET us.Active=1 FROM UsersSystems us INNER JOIN Users u ON us.IdUsers = u.Id WHERE u.IdUserIdentity=@idUserIdentity AND us.IdSystems = @idSystems", parameters);
            }
        }

        public async Task AddAsync(User obj)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@IdUserIdentity", obj.IdUserIdentity, DbType.String, ParameterDirection.Input);
            parameters.Add("@Email", obj.Email, DbType.String, ParameterDirection.Input);
            parameters.Add("@FirstName", obj.FirstName, DbType.String, ParameterDirection.Input);
            parameters.Add("@SurName", obj.SurName, DbType.String, ParameterDirection.Input);
            parameters.Add("@Avatar", obj.Avatar, DbType.Binary, ParameterDirection.Input);
            await base.AddAsync("USP_INS_Users", parameters, CommandType.StoredProcedure);
        }

        public async Task UpdateAsync(User obj)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@Id", obj.Id, DbType.Int64, ParameterDirection.Input);
            parameters.Add("@IdUserIdentity", obj.IdUserIdentity, DbType.String, ParameterDirection.Input);
            parameters.Add("@Email", obj.Email, DbType.String, ParameterDirection.Input);
            parameters.Add("@FirstName", obj.FirstName, DbType.String, ParameterDirection.Input);
            parameters.Add("@SurName", obj.SurName, DbType.String, ParameterDirection.Input);
            parameters.Add("@Avatar", obj.Avatar, DbType.Binary, ParameterDirection.Input);
            parameters.Add("@Active", obj.Active, DbType.Boolean, ParameterDirection.Input);
            await base.UpdateAsync("USP_UPD_Users", parameters, CommandType.StoredProcedure);
        }

        public async Task AddSystemAsync(string idUserIdentity, int idSystems)
        {
            using (var dbConnection = GetConnection())
            {
                var parameters = new DynamicParameters();
                parameters.Add("@IdUserIdentity", idUserIdentity, DbType.String, ParameterDirection.Input);
                parameters.Add("@IdSystems", idSystems, DbType.Int64, ParameterDirection.Input);

                dbConnection.Open();
                await dbConnection.ExecuteAsync("USP_INS_UsersSystems", parameters, null, null, commandType: CommandType.StoredProcedure);
            }
        }
    }
}
