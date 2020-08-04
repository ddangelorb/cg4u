using System.Collections.Generic;
using CG4U.Core.Common.Domain.Models;

namespace CG4U.Core.Services.Controllers
{
    public class UsersRequest
    {
        public User UserLoggedIn { get; set; }
        public List<User> ListUsersViewModel { get; set; }

        public UsersRequest()
        {
            ListUsersViewModel = new List<User>();
        }
    }
}
