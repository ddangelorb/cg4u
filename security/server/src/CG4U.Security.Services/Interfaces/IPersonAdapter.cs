using System.Threading.Tasks;
using CG4U.Security.Domain.Configuration;
using CG4U.Security.Domain.Persons;

namespace CG4U.Security.Services.Interfaces
{
    public interface IPersonAdapter
    {
        Task<bool> CreatePersonGroupAsync(PersonGroup personGroup);
        Task<string> CreatePersonAsync(Person person);
        Task<string> AddPersonFaceAsync(Person person, byte[] image);
        Task<bool> TrainPersonGroupAsync(PersonGroup personGroup);
        Task<Status> GetTrainingStatusPersonGroupAsync(PersonGroup personGroup);
    }
}
