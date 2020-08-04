using System.Threading.Tasks;
using CG4U.Donate.ClientApp.Med.Services.Root;

namespace CG4U.Donate.ClientApp.Med.Services.Interfaces
{
    public interface IAccountService
    {
        Task<RootRegister> RegisterAsync(ViewModels.LoginViewModel loginViewModel);
        Task<RootLogin> LoginAsync(ViewModels.LoginViewModel loginViewModel);
    }
}
