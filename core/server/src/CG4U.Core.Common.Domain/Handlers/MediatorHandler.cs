using System.Threading.Tasks;
using CG4U.Core.Common.Domain.Interfaces;
using CG4U.Core.Common.Domain.Messages;
using MediatR;

namespace CG4U.Core.Common.Domain.Handlers
{
    public class MediatorHandler : IMediatorHandler
    {
        private readonly IMediator _mediator;

        public MediatorHandler(IMediator mediator)
        {
            _mediator = mediator;
        }

        public Task PublishEvent<T>(T vEvent) where T : Event
        {
            return _mediator.Publish(vEvent);
        }

        public Task SendCommand<T>(T vCommand) where T : Command
        {
            return _mediator.Publish(vCommand);
        }
    }
}
