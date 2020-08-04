using System;
using CG4U.Core.Common.Domain.Messages;
using CG4U.Security.Domain.Persons.Models;
using Microsoft.Extensions.Logging;

namespace CG4U.Security.Domain.Persons.Events
{
    public class PersonGroupAddedEvent : Event
    {
        public PersonGroupModel PersonGroupModel { get; set; }

        public PersonGroupAddedEvent(ILogger logger, PersonGroupModel personGroupModel)
            : base(logger)
        {
            PersonGroupModel = personGroupModel;
        }

        public override void NotifyEventHandled()
        {
            Logger.LogInformation(
                string.Format(
                    "{0} __ Person Group Added successfully. PersonGroup.Id {1}, PersonGroup.IdApi {2}, PersonGroup.Name {3}",
                    DateTime.Now.ToString("G"), PersonGroupModel.Id, PersonGroupModel.IdApi, PersonGroupModel.Name
                )
            );
        }
    }
}
