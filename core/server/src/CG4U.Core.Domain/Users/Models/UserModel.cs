using System;
using System.Collections.Generic;
using CG4U.Core.Common.Domain.Interfaces;
using CG4U.Core.Common.Domain.Models;
using Microsoft.AspNetCore.Identity;

namespace CG4U.Core.Domain.Users.Models
{
    public class UserModel : EntityModel<UserModel>
    {
        public string IdUserIdentity { get; set; }
        public int IdSystems { get; set; }
        public int IdLanguages { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string PasswordConfirmation { get; set; }
        public string FirstName { get; set; }
        public string SurName { get; set; }
        public byte[] Avatar { get; set; }
        public int Authenticated { get; set; }
        public DateTime DtExpAuth { get; set; }
        public IdentityUser IdentityUser { get; set; }
        public ICollection<UserRoles> Roles { get; set; }
    }
}
