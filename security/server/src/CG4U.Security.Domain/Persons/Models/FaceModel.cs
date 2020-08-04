using System;
using CG4U.Core.Common.Domain.Models;

namespace CG4U.Security.Domain.Persons.Models
{
    public class FaceModel : EntityModel<FaceModel>
    {
        public int IdPersons { get; set; }
        public Guid FaceId { get; set; }
        public byte[] Image { get; set; }
    }
}
