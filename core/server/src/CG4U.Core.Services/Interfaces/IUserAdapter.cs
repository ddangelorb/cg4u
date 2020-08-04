using System.Collections.Generic;
using System.Threading.Tasks;
using CG4U.Core.Services.ViewModels;
using Microsoft.AspNetCore.Identity;

namespace CG4U.Core.Services.Interfaces
{
    public interface IUserAdapter
    {
        ICollection<string> GetErrors();
        Task<UserViewModel> GetDbByIdentityAsync(IdentityUser identityUser);
        Task<bool> IsUserHaveAccessSystem(IdentityUser identityUser, int idSystems);
        Task<bool> AddDbAsync(IdentityUser identityUser, UserViewModel model);
        Task<bool> AddSystemDbAsync(IdentityUser identityUser, UserViewModel model);
        Task<bool> EnableDbAsync(IdentityUser identityUser, int idSystems);
        Task<object> GenerateJwtTokenAsync(IdentityUser identityUser, UserViewModel login);
    }
}
