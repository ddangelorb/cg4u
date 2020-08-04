using System.Threading.Tasks;
using CG4U.Security.Services.Data;

namespace CG4U.Security.Services.Interfaces
{
    public interface IComputerVisionAdapter
    {
        Task<string> DescribeImageAsync(string language, byte[] image, ConnectionApiData connectionApiData);
        Task<string> RecognizeTextAsync(string mode, byte[] image, ConnectionApiData connectionApiData);
        Task<string> OCRAsync(string language, byte[] image, ConnectionApiData connectionApiData);
    }
}
