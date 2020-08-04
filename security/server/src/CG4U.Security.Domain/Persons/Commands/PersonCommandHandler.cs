using System.Threading.Tasks;
using AutoMapper;
using CG4U.Core.Common.Domain.Handlers;
using CG4U.Core.Common.Domain.Interfaces;
using CG4U.Core.Common.Domain.Notifications;
using CG4U.Security.Domain.Persons.Events;
using CG4U.Security.Domain.Persons.Models;
using CG4U.Security.Domain.Persons.Repository;
using MediatR;
using Microsoft.Extensions.Logging;

namespace CG4U.Security.Domain.Persons.Commands
{
    public class PersonCommandHandler : CommandHandler,
        IAsyncNotificationHandler<AddPersonCommand>,
        IAsyncNotificationHandler<AddPersonGroupCommand>,
        IAsyncNotificationHandler<AddPersonFaceCommand>
    {
        private readonly IMediatorHandler _mediator;
        private readonly IPersonRepository _personRepository;

        public PersonCommandHandler(IMediatorHandler mediator,
                                    IPersonRepository personRepository,
                                    ILogger<PersonCommandHandler> logger,
                                    INotificationHandler<DomainNotification> notifications)
            : base(mediator, (DomainNotificationHandler)notifications, logger)
        {
            _mediator = mediator;
            _personRepository = personRepository;
        }

        public async Task Handle(AddPersonCommand notification)
        {
            var entityCurrent = Mapper.Map<PersonModel, Person>(notification.PersonModel);

            if (!IsEntityValid(entityCurrent)) return;

            var personGroup = await _personRepository.GetPersonGroupByIdAsync(entityCurrent.PersonGroup.Id);
            if (personGroup == null)
            {
                NotifyErrorValidations("AddPersonCommand", "PersonGroup not found");
                return;
            }

            await _personRepository.AddAsync(entityCurrent);

            if (Commit())
                await _mediator.PublishEvent(new PersonAddedEvent(_logger, Mapper.Map<PersonModel>(notification.PersonModel)));
        }

        public async Task Handle(AddPersonGroupCommand notification)
        {
            var entityCurrent = Mapper.Map<PersonGroupModel, PersonGroup>(notification.PersonGroupModel);

            if (!IsEntityValid(entityCurrent)) return;

            await _personRepository.AddPersonGroupAsync(entityCurrent);

            if (Commit())
                await _mediator.PublishEvent(new PersonGroupAddedEvent(_logger, Mapper.Map<PersonGroupModel>(notification.PersonGroupModel)));
        }

        public async Task Handle(AddPersonFaceCommand notification)
        {
            var entityCurrent = Mapper.Map<FaceModel, Face>(notification.FaceModel);

            if (!IsEntityValid(entityCurrent)) return;

            var person = await _personRepository.GetByIdAsync(entityCurrent.IdPersons);
            if (person == null)
            {
                NotifyErrorValidations("AddPersonFaceCommand", "Person not found");
                return;
            }

            await _personRepository.AddPersonFaceAsync(entityCurrent);

            if (Commit())
                await _mediator.PublishEvent(new PersonFaceAddedEvent(_logger, Mapper.Map<FaceModel>(notification.FaceModel)));
        }
    }
}
