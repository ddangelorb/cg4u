using System;
using System.Collections.Generic;
using CG4U.Core.Common.Domain.Models;

namespace CG4U.Security.Domain.Configuration.Models
{
    public class VideoCameraModel : EntityModel<VideoCameraModel>
    {
        public int IdPersonGroups { get; set; }
        public string IdPersonGroupsAPI { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public IList<AnalyzeRequest> AnalyzesRequests { get; set; }

        public VideoCameraModel()
        {
            AnalyzesRequests = new List<AnalyzeRequest>();
        }
    }
}
