using System.Threading.Tasks;
using CG4U.Core.Common.Domain.Interfaces;
using CG4U.Core.Common.Domain.Models;

namespace CG4U.Core.Domain.Users.Repository
{
    public interface IUserRepository : IRepository<User>
    {
        Task EnableByIdUserIdentityAsync(string idUserIdentity, int idSystems);
        Task<User> GetByIdUserIdentityAsync(string idUserIdentity);
        Task<bool> IsUserHasAccessSystem(string idUserIdentity, int idSystems);
        Task AddSystemAsync(string idUserIdentity, int idSystems);
    }
}
