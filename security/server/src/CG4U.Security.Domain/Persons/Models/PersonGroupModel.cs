using System;
using System.Collections.Generic;
using CG4U.Core.Common.Domain.Models;
using CG4U.Security.Domain.Configuration.Models;

namespace CG4U.Security.Domain.Persons.Models
{
    public class PersonGroupModel : EntityModel<PersonGroupModel>
    {
        public int IdCustomers { get; set; }
        public Guid IdApi { get; set; }
        public string Name { get; set; }
        public ICollection<AlertModel> Alerts { get; set; }

        public PersonGroupModel()
        {
            Alerts = new List<AlertModel>();
        }
    }
}
