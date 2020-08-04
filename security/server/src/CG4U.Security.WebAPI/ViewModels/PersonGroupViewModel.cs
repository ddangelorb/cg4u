using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using CG4U.Core.Common.Domain.Models;
using CG4U.Core.Services.ViewModels;

namespace CG4U.Security.WebAPI.ViewModels
{
    public class PersonGroupViewModel : EntityModel<PersonGroupViewModel>
    {
        [Display(Name = "PersonGroup.IdCustomers.DisplayName")]
        [Required(ErrorMessage = "PersonGroup.IdCustomers.NotEmpty")]
        [Range(1, int.MaxValue, ErrorMessage = "PersonGroup.IdCustomers.GreaterThanZero")]
        public int IdCustomers { get; set; }

        [Display(Name = "PersonGroup.IdApi.DisplayName")]
        [Required(ErrorMessage = "PersonGroup.IdApi.NotEmpty")]
        public Guid IdApi { get; set; }

        [Display(Name = "PersonGroup.Name.DisplayName")]
        [Required(ErrorMessage = "PersonGroup.Name.NotEmpty")]
        [MinLength(2, ErrorMessage = "PersonGroup.Name.LengthMin2")]
        [MaxLength(100, ErrorMessage = "PersonGroup.Name.LengthMax100")]
        public string Name { get; set; }

        [Required, ValidateListOfObject(ErrorMessage = "PersonGroup.Alerts.NotEmpty")]
        public ICollection<AlertViewModel> Alerts { get; set; }

        public PersonGroupViewModel()
        {
            Alerts = new List<AlertViewModel>();
        }
    }
}
