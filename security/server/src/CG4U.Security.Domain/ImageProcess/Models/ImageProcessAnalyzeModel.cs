using System;
using CG4U.Core.Common.Domain.Models;

namespace CG4U.Security.Domain.ImageProcess.Models
{
    public class ImageProcessAnalyzeModel : EntityModel<ImageProcessAnalyzeModel>
    {
        public Guid IdReference { get; set; }
        public int IdImageProcesses { get; set; }
        public int IdAnalyzesRequestsVideoCameras { get; set; }
        public DateTime DtAnalyze { get; set; }
        public string ReturnResponseType { get; set; }
        public string ReturnResponse { get; set; }
        public int Commited { get; set; }
    }
}
