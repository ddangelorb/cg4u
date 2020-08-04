using CG4U.Core.Common.Domain.Models;

namespace CG4U.Security.Domain.Configuration.Models
{
    public class AnalyzeRequestModel : EntityModel<AnalyzeRequestModel>
    {
        public int IdBillableProcesses { get; set; }
        public int IdLanguages { get; set; }
        public int AnalyzeOrder { get; set; }
        public int TypeCode { get; set; }
        public string TypeName { get; set; }
        public int IdAnalyzesRequestsVideoCameras { get; set; }
        public string Location { get; set; }
        public string SubscriptionKey { get; set; }
    }
}
