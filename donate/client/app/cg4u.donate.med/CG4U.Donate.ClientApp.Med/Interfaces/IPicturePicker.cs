using System;
using System.IO;
using System.Threading.Tasks;

namespace CG4U.Donate.ClientApp.Med.Interfaces
{
    public interface IPicturePicker
    {
        Task<Stream> GetImageStreamAsync();
    }
}
