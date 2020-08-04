using System.Collections.Generic;
using FluentValidation;
using FluentValidation.Results;

namespace CG4U.Core.Common.Domain.Models
{
    public abstract class Entity<T> : AbstractValidator<T> where T : Entity<T>
    {
        public int Id { get; set; }
        public int Active { get; set; }
        public ValidationResult ValidationResult { get; set; }

        protected Entity()
        {
            ValidationResult = new ValidationResult();
        }

        public abstract bool IsValid();

        public override bool Equals(object obj)
        {
            var compareTo = obj as Entity<T>;

            if (ReferenceEquals(this, compareTo)) return true;
            if (ReferenceEquals(null, compareTo)) return false;

            return Id.Equals(compareTo.Id);
        }

        public static bool operator ==(Entity<T> a, Entity<T> b)
        {
            if (ReferenceEquals(a, null) && ReferenceEquals(b, null))
                return true;

            if (ReferenceEquals(a, null) || ReferenceEquals(b, null))
                return false;

            return a.Equals(b);
        }

        public static bool operator !=(Entity<T> a, Entity<T> b)
        {
            return !(a == b);
        }

        public override int GetHashCode()
        {
            return (GetType().GetHashCode() * 907) + Id.GetHashCode();
        }

        public override string ToString()
        {
            return GetType().Name + "[Id = " + Id + "]";
        }

        protected void AddOtherErrorList(IList<ValidationFailure> otherErrorList)
        {
            if (otherErrorList != null)
            {
                if (otherErrorList.Count == 0) return;

                foreach (var err in otherErrorList)
                {
                    ValidationResult.Errors.Add(err);
                }
            }
        }
    }
}
