using System;
using CG4U.Core.Common.Domain.Models;

namespace CG4U.Security.Domain.Configuration.Models
{
    public class AlertModel : EntityModel<AlertModel>
    {
        public int IdAnalyzesRequests { get; set; }
        public int TypeCode { get; set; }
        public string Message { get; set; }
        public string ProcessingMethod { get; set; }
        public string ProcessingParam { get; set; }
    }
}
