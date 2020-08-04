using CG4U.Core.Common.Domain.Models;

namespace CG4U.Security.Domain.Persons.Models
{
    public class CarModel : EntityModel<CarModel>
    {
        public int IdPersons { get; set; }
        public string PlateCode { get; set; }
    }
}
