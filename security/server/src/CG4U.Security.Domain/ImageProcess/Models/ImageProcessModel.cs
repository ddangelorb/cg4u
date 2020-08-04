using System;
using System.Collections.Generic;
using CG4U.Core.Common.Domain.Models;

namespace CG4U.Security.Domain.ImageProcess.Models
{
    public class ImageProcessModel : EntityModel<ImageProcessModel>
    {
        public int IdVideoCameras { get; set; }
        public Guid IdReference { get; set; }
        public byte[] ImageFile { get; set; }
        public string ImageName { get; set; }
        public string IpUserRequest { get; set; }
        public string VideoPath { get; set; }
        public int SecondsToStart { get; set; }
        public DateTime DtProcess { get; set; }
        public ICollection<ImageProcessAnalyzeModel> Analyzes { get; set; }

        public ImageProcessModel()
        {
            Analyzes = new List<ImageProcessAnalyzeModel>();
        }
    }
}
