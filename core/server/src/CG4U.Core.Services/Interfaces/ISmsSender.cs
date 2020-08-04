using System.Threading.Tasks;
namespace CG4U.Core.Services.Interfaces
{
    public interface ISmsSender
    {
        Task SendSmsAsync(string number, string message);
    }
}
