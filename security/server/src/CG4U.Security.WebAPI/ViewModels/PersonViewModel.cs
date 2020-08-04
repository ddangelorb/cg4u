using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using CG4U.Core.Common.Domain.Models;
using CG4U.Core.Services.ViewModels;

namespace CG4U.Security.WebAPI.ViewModels
{
    public class PersonViewModel : EntityModel<PersonViewModel>
    {
        [Required(ErrorMessage = "Person.PersonGroup.NotEmpty")]
        [ValidateObject(ErrorMessage = "Person.PersonGroup.NotEmpty")]
        public PersonGroupViewModel PersonGroup { get; set; }

        [Display(Name = "Person.IdApi.DisplayName")]
        public Guid IdApi { get; set; }

        [Display(Name = "Person.IdUsers.DisplayName")]
        public int IdUsers { get; set; }

        [Display(Name = "Person.Name.DisplayName")]
        [Required(ErrorMessage = "Person.Name.NotEmpty")]
        [MinLength(2, ErrorMessage = "Person.Name.LengthMin2")]
        [MaxLength(255, ErrorMessage = "Person.Name.LengthMax255")]
        public string Name { get; set; }

        [Required, ValidateListOfObject(ErrorMessage = "Person.Cars.NotEmpty")]
        public ICollection<CarViewModel> Cars { get; set; }

        [Required, ValidateListOfObject(ErrorMessage = "Person.Faces.NotEmpty")]
        public ICollection<FaceViewModel> Faces { get; set; }

        public PersonViewModel()
        {
            PersonGroup = new PersonGroupViewModel();
            Cars = new List<CarViewModel>();
            Faces = new List<FaceViewModel>();
        }
    }
}
