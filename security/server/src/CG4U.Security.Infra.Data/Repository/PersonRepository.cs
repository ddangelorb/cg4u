using System;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using CG4U.Core.Common.Domain.Models;
using CG4U.Security.Domain.Configuration;
using CG4U.Security.Domain.Persons;
using CG4U.Security.Domain.Persons.Repository;
using Dapper;
using Microsoft.Extensions.Options;

namespace CG4U.Security.Infra.Data.Repository
{
    public class PersonRepository : Repository<Person>, IPersonRepository
    {
        public PersonRepository(IOptions<DbConnection> dbConnection)
            : base("Persons", dbConnection.Value.DefaultConnection)
        {
        }

        public async Task<Person> GetByIdUserAsync(int idUser)
        {
            using (IDbConnection dbConnection = GetConnection())
            {
                var parameters = new DynamicParameters();
                parameters.Add("@IdUsers", idUser, DbType.Int64, ParameterDirection.Input);

                dbConnection.Open();
                var query = await dbConnection.QueryAsync<Person, PersonGroup, Alert, Face, Car, Person>(
                    "USP_SEL_PersonsByIdUser",
                    map: (person, personGroup, alert, face, car) =>
                    {
                        personGroup.AddAlert(alert);
                        person.PersonGroup = personGroup;
                        person.AddFace(face);
                        person.AddCar(car);
                        return person;
                    },
                    param: parameters,
                    commandType: CommandType.StoredProcedure
                );

                return await query.ToAsyncEnumerable().Distinct().FirstOrDefault();
            }
        }

        public async Task<Person> GetByIdApiAsync(string idApi)
        {
            using (IDbConnection dbConnection = GetConnection())
            {
                var parameters = new DynamicParameters();
                parameters.Add("@idApi", idApi, DbType.String, ParameterDirection.Input);

                dbConnection.Open();
                return await dbConnection.QueryFirstOrDefaultAsync<Person>("SELECT * FROM Persons WHERE IdApi=@idApi AND Active=1", parameters);
            }
        }

        public async Task<PersonGroup> GetPersonGroupByIdAsync(int idPersonGroup)
        {
            using (IDbConnection dbConnection = GetConnection())
            {
                var parameters = new DynamicParameters();
                parameters.Add("@Id", idPersonGroup, DbType.Int64, ParameterDirection.Input);

                dbConnection.Open();
                var query = await dbConnection.QueryAsync<PersonGroup, Alert, PersonGroup>(
                    "USP_SEL_PersonGroupsById",
                    map: (personGroup, alert) =>
                    {
                        personGroup.AddAlert(alert);
                        return personGroup;
                    },
                    param: parameters,
                    commandType: CommandType.StoredProcedure
                );

                return await query.ToAsyncEnumerable().Distinct().FirstOrDefault();
            }
        }

        public async Task<PersonGroup> GetPersonGroupByIdApiAsync(Guid idApi)
        {
            using (IDbConnection dbConnection = GetConnection())
            {
                var parameters = new DynamicParameters();
                parameters.Add("@idApi", idApi, DbType.Guid, ParameterDirection.Input);

                dbConnection.Open();
                return await dbConnection.QueryFirstOrDefaultAsync<PersonGroup>("SELECT * FROM PersonGroups WHERE IdApi=@idApi AND Active=1", parameters);
            }
        }

        public async Task<bool> IsCarBelongToPersonGroupAsync(int idPersonGroup, string plateCode)
        {
            using (IDbConnection dbConnection = GetConnection())
            {
                var parameters = new DynamicParameters();
                parameters.Add("@idPersonGroup", idPersonGroup, DbType.Int64, ParameterDirection.Input);
                parameters.Add("@plateCode", plateCode, DbType.String, ParameterDirection.Input);

                dbConnection.Open();
                var car = await dbConnection.QueryFirstOrDefaultAsync<Car>("SELECT pc.* FROM PersonCars pc INNER JOIN Persons p ON pc.IdPersons = p.Id WHERE p.IdPersonGroups=@idPersonGroup AND pc.PlateCode = @plateCode AND pc.Active=1 AND p.Active = 1", parameters);
                return car != null;
            }
        }

        public async Task AddAsync(Person obj)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@IdPersonGroups", obj.PersonGroup.Id, DbType.Int64, ParameterDirection.Input);
            parameters.Add("@IdApi", obj.IdApi, DbType.Guid, ParameterDirection.Input);
            parameters.Add("@IdUsers", obj.IdUsers, DbType.Int64, ParameterDirection.Input);
            parameters.Add("@Name", obj.Name, DbType.String, ParameterDirection.Input);
            await base.AddAsync("USP_INS_Persons", parameters, CommandType.StoredProcedure);
        }

        public async Task AddPersonGroupAsync(PersonGroup personGroup)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@IdCustomers", personGroup.IdCustomers, DbType.Int64, ParameterDirection.Input);
            parameters.Add("@IdApi", personGroup.IdApi, DbType.Guid, ParameterDirection.Input);
            parameters.Add("@Name", personGroup.Name, DbType.String, ParameterDirection.Input);
            await base.AddAsync("USP_INS_PersonGroups", parameters, CommandType.StoredProcedure);
        }

        public async Task AddPersonFaceAsync(Face face)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@IdPersons", face.IdPersons, DbType.Int64, ParameterDirection.Input);
            parameters.Add("@FaceId", face.FaceId, DbType.Guid, ParameterDirection.Input);
            await base.AddAsync("USP_INS_PersonFaces", parameters, CommandType.StoredProcedure);
        }

        public async Task AddPersonCarAsync(Car car)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@IdPersons", car.IdPersons, DbType.Int64, ParameterDirection.Input);
            parameters.Add("@PlateCode", car.PlateCode, DbType.String, ParameterDirection.Input);
            await base.AddAsync("USP_INS_PersonCars", parameters, CommandType.StoredProcedure);
        }

        public async Task UpdateAsync(Person obj)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@Id", obj.Id, DbType.Int64, ParameterDirection.Input);
            parameters.Add("@IdPersonGroups", obj.PersonGroup.Id, DbType.Int64, ParameterDirection.Input);
            parameters.Add("@IdApi", obj.IdApi, DbType.Guid, ParameterDirection.Input);
            parameters.Add("@IdUsers", obj.IdUsers, DbType.Int64, ParameterDirection.Input);
            parameters.Add("@Name", obj.Name, DbType.String, ParameterDirection.Input);
            parameters.Add("@Active", obj.Active, DbType.Int16, ParameterDirection.Input);
            await base.UpdateAsync("USP_UPD_Persons", parameters, CommandType.StoredProcedure);
        }

        public async Task UpdatePersonGroupAsync(PersonGroup personGroup)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@Id", personGroup.Id, DbType.Int64, ParameterDirection.Input);
            parameters.Add("@IdCustomers", personGroup.IdCustomers, DbType.Int64, ParameterDirection.Input);
            parameters.Add("@IdApi", personGroup.IdApi, DbType.Guid, ParameterDirection.Input);
            parameters.Add("@Name", personGroup.Name, DbType.String, ParameterDirection.Input);
            parameters.Add("@Active", personGroup.Active, DbType.Int16, ParameterDirection.Input);
            await base.UpdateAsync("USP_UPD_PersonGroups", parameters, CommandType.StoredProcedure);
        }
    }
}
