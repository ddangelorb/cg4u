using System;
using System.Collections.Generic;
using CG4U.Core.Common.Domain.Models;
using CG4U.Security.Domain.Configuration;
using FluentValidation;

namespace CG4U.Security.Domain.Persons
{
    public class PersonGroup : Entity<PersonGroup>
    {
        public int IdCustomers { get; set; }
        public Guid IdApi { get; set; }
        public string Name { get; set; }
        public ICollection<Alert> Alerts { get; set; }

        public PersonGroup()
        {
            Alerts = new List<Alert>();
        }

        public void AddAlert(Alert alert)
        {
            Alerts.Add(alert);
        }

        public override bool IsValid()
        {
            RuleFor(c => c.IdCustomers)
                .GreaterThan(0).WithMessage("PersonGroup.IdCustomers.GreaterThanZero");

            RuleFor(c => c.IdApi)
                .NotEmpty().WithMessage("PersonGroup.IdApi.NotEmpty");

            RuleFor(c => c.Name)
                .NotEmpty().WithMessage("PersonGroup.Name.NotEmpty")
                .Length(2, 100).WithMessage("PersonGroup.Name.LengthBetween2And100");

            ValidateAlerts();

            return ValidationResult.IsValid;
        }

        private void ValidateAlerts()
        {
            foreach (var a in Alerts)
            {
                if (!a.IsValid())
                    AddOtherErrorList(a.ValidationResult.Errors);
            }
        }
    }
}
