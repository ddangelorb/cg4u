using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CG4U.Core.Services.ViewModels
{
    public class ValidateListOfObjectAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            foreach (var itemValue in value as IEnumerable)
            {
                var results = new List<ValidationResult>();
                var context = new ValidationContext(itemValue, null, null);

                Validator.TryValidateObject(itemValue, context, results, true);

                if (results.Count != 0)
                {
                    var compositeResults = new CompositeValidationResult(ErrorMessage);
                    results.ForEach(compositeResults.AddResult);
                    return compositeResults;
                }
            }

            return ValidationResult.Success;
        }
    }
}
