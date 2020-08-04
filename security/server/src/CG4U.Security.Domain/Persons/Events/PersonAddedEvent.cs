using System;
using CG4U.Core.Common.Domain.Messages;
using CG4U.Security.Domain.Persons.Models;
using Microsoft.Extensions.Logging;

namespace CG4U.Security.Domain.Persons.Events
{
    public class PersonAddedEvent : Event
    {
        public PersonModel PersonModel { get; set; }

        public PersonAddedEvent(ILogger logger, PersonModel personModel)
            : base(logger)
        {
            PersonModel = personModel;
        }

        public override void NotifyEventHandled()
        {
            Logger.LogInformation(
                string.Format(
                    "{0} __ Person Added successfully. Person.Id {1}, Person.IdApi {2}, Person.Name {3}",
                    DateTime.Now.ToString("G"), PersonModel.Id, PersonModel.IdApi, PersonModel.Name
                )
            );
        }
    }
}
