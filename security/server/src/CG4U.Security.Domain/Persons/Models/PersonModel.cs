using System;
using System.Collections.Generic;
using CG4U.Core.Common.Domain.Models;

namespace CG4U.Security.Domain.Persons.Models
{
    public class PersonModel : EntityModel<PersonModel>
    {
        public PersonGroupModel PersonGroup { get; set; }
        public Guid IdApi { get; set; }
        public int IdUsers { get; set; }
        public string Name { get; set; }
        public ICollection<CarModel> Cars { get; set; }
        public ICollection<FaceModel> Faces { get; set; }

        public PersonModel()
        {
            PersonGroup = new PersonGroupModel();
            Cars = new List<CarModel>();
            Faces = new List<FaceModel>();
        }
    }
}
