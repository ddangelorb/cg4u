using System.Threading.Tasks;
using CG4U.Security.ClientApp.Services.Roots;
using CG4U.Security.ClientApp.ViewModels;

namespace CG4U.Security.ClientApp.Services.Interfaces
{
    public interface IAccountService
    {
        Task<RootRegister> RegisterAsync(LoginViewModel loginViewModel);
        Task<RootLogin> LoginAsync(LoginViewModel loginViewModel);
    }
}
