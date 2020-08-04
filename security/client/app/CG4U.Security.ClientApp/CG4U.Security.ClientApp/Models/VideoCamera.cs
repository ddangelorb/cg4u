using System.IO;
using Xamarin.Forms;

namespace CG4U.Security.ClientApp.Models
{
    public class VideoCamera
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string IngestStreamingUri { get; set; }
        public byte[] ImageFile { get; set; }
        public ImageSource ImageFileSorce
        {
            get
            {
                return ImageSource.FromStream(() => new MemoryStream(ImageFile));
            }
        }
    }
}
