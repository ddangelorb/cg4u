using System.Threading.Tasks;
using CG4U.Core.Common.Domain.Messages;

namespace CG4U.Core.Common.Domain.Interfaces
{
    public interface IMediatorHandler
    {
        Task PublishEvent<T>(T vEvent) where T : Event;
        Task SendCommand<T>(T vCommand) where T : Command;
    }
}
