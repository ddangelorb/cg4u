using System;
using System.Threading.Tasks;
using CG4U.Core.Common.Domain.Interfaces;

namespace CG4U.Security.Domain.Persons.Repository
{
    public interface IPersonRepository : IRepository<Person>
    {
        Task<Person> GetByIdUserAsync(int idUser);
        Task<Person> GetByIdApiAsync(string idApi);
        Task<PersonGroup> GetPersonGroupByIdAsync(int idPersonGroup);
        Task<PersonGroup> GetPersonGroupByIdApiAsync(Guid idApi);
        Task<bool> IsCarBelongToPersonGroupAsync(int idPersonGroup, string plateCode);
        Task AddPersonGroupAsync(PersonGroup personGroup);
        Task AddPersonFaceAsync(Face face);
        Task AddPersonCarAsync(Car car);
        Task UpdatePersonGroupAsync(PersonGroup personGroup);
    }
}
