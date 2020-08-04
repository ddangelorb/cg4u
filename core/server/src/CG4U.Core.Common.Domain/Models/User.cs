using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using CG4U.Core.Common.Domain.Interfaces;
using FluentValidation;
using Microsoft.AspNetCore.Identity;

namespace CG4U.Core.Common.Domain.Models
{
    public class User : Entity<User>
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

        public override bool IsValid()
        {
            RuleFor(c => c.IdSystems)
                .GreaterThan(0).WithMessage("User.IdSystems.GreaterThanZero")
                .Must(idSystems => Enum.IsDefined(typeof(Systems), idSystems) == true)
                .WithMessage("User.IdSystems.MemberOfSystemsEnum");

            RuleFor(c => c.Email)
                .NotEmpty().WithMessage("User.Email.NotEmpty")
                .Length(6, 320).WithMessage("User.Email.LengthBetween6And320")
                .Matches(@"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z", RegexOptions.IgnoreCase)
                    .WithMessage("User.Email.InvalidFormat");

            RuleFor(c => c.FirstName)
                .NotEmpty().WithMessage("User.FirstName.NotEmpty")
                .Length(2, 50).WithMessage("User.FirstName.LengthBetween2And50");

            RuleFor(c => c.SurName)
                .NotEmpty().WithMessage("User.SurName.NotEmpty")
                .Length(2, 206).WithMessage("User.SurName.LengthBetween2And206");

            return ValidationResult.IsValid;
        }
    }
}
