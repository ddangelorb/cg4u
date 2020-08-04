using System.ComponentModel.DataAnnotations;

namespace CG4U.Core.Common.Domain.Interfaces
{
    public enum Languages
    {
        [Display(Name = "pt", Description = "Portuguese")]
        BrazilianPortuguese = 1,

        [Display(Name = "en", Description = "English")]
        English = 2,

        [Display(Name = "es", Description = "Spanish")]
        Spanish = 3,

        [Display(Name = "fr", Description = "French")]
        French = 4,

        [Display(Name = "it", Description = "Italian")]
        Italian = 5,

        [Display(Name = "de", Description = "German")]
        German = 6
    }
}
