using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Text;
using System.Threading.Tasks;
using Dapper;

namespace CG4U.Core.Common.Domain.Models
{
    public abstract class Repository<TEntity>
    {
        private string _dbConnection;
        protected string _sqlTableName;

        protected Repository(string sqlTableName, string dbConnection)
        {
            _dbConnection = dbConnection;
            _sqlTableName = sqlTableName;
        }

        protected IDbConnection GetConnection()
        {
            return new SqlConnection(_dbConnection);
        }

        public async Task DisableAsync(int id)
        {
            using (var dbConnection = GetConnection())
            {
                var parameters = new DynamicParameters();
                parameters.Add("@id", id, DbType.Int64, ParameterDirection.Input);

                dbConnection.Open();
                await dbConnection.ExecuteAsync(string.Format("UPDATE {0} SET Active=0 WHERE Id=@id", _sqlTableName), parameters);
            }
        }

        public virtual async Task<TEntity> GetByIdAsync(int id)
        {
            using (var dbConnection = GetConnection())
            {
                var parameters = new DynamicParameters();
                parameters.Add("@id", id, DbType.Int64, ParameterDirection.Input);

                dbConnection.Open();
                return await dbConnection.QueryFirstOrDefaultAsync<TEntity>(string.Format("SELECT * FROM {0} WHERE Id=@id AND Active=1", _sqlTableName), parameters);
            }
        }

        public virtual async Task<IEnumerable<TEntity>> ListAllAsync()
        {
            using (var dbConnection = GetConnection())
            {
                dbConnection.Open();
                return await dbConnection.QueryAsync<TEntity>(string.Format("SELECT * FROM {0} WHERE Active=1", _sqlTableName));
            }
        }

        public async Task<bool> IsActiveAsync(int id)
        {
            using (var dbConnection = GetConnection())
            {
                var parameters = new DynamicParameters();
                parameters.Add("@Id", id, DbType.Int64, ParameterDirection.Input);

                dbConnection.Open();
                var usr = await dbConnection.QueryAsync<object>(string.Format("SELECT * FROM {0} WHERE Id=@Id AND Active = 1", _sqlTableName), parameters);

                return usr != null;
            }
        }

        protected async Task AddAsync(string sqlStatement, DynamicParameters parameters, CommandType cmdType)
        {
            using (var dbConnection = GetConnection())
            {
                dbConnection.Open();
                await dbConnection.ExecuteAsync(sqlStatement, parameters, null, null, commandType: cmdType);
            }
        }

        protected async Task UpdateAsync(string sqlStatement, DynamicParameters parameters, CommandType cmdType)
        {
            using (var dbConnection = GetConnection())
            {
                dbConnection.Open();
                await dbConnection.ExecuteAsync(sqlStatement, parameters, null, null, commandType: cmdType);
            }
        }

        protected string RemoveAccentLetters(string source)
        {
            var normalizedString = source.Normalize(NormalizationForm.FormD);
            var stringBuilder = new StringBuilder();

            foreach (var c in normalizedString)
            {
                var unicodeCategory = CharUnicodeInfo.GetUnicodeCategory(c);
                if (unicodeCategory != UnicodeCategory.NonSpacingMark)
                    stringBuilder.Append(c);
            }

            return stringBuilder.ToString().Normalize(NormalizationForm.FormC);
        }
    }
}
