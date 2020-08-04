using FluentValidation;
using FluentValidation.Results;

namespace CG4U.Core.Common.Domain.Models
{
    public abstract class EntityModel<T> where T : EntityModel<T>
    {
        public int Id { get; set; }
        public int Active { get; set; }
        public ValidationResult ValidationResult { get; set; }
        public CascadeMode CascadeMode { get; set; }

        protected EntityModel()
        {
            ValidationResult = new ValidationResult();
        }
    }
}
