using System.Threading.Tasks;
using CG4U.Security.ClientApp.Services.Roots;

namespace CG4U.Security.ClientApp.Services.Interfaces
{
    public interface IPersonService
    {
        Task<RootPerson> GetByIdUsers(int idUsers);
    }
}
