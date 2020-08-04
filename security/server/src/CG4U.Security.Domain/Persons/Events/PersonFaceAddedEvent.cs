using System;
using CG4U.Core.Common.Domain.Messages;
using CG4U.Security.Domain.Persons.Models;
using Microsoft.Extensions.Logging;

namespace CG4U.Security.Domain.Persons.Events
{
    public class PersonFaceAddedEvent : Event
    {
        public FaceModel FaceModel { get; set; }

        public PersonFaceAddedEvent(ILogger logger, FaceModel faceModel)
            : base(logger)
        {
            FaceModel = faceModel;
        }

        public override void NotifyEventHandled()
        {
            Logger.LogInformation(
                string.Format(
                    "{0} __ Face Person Added successfully. Face.Id {1}, Face.IdPersons {2}, Face.FaceId {3}",
                    DateTime.Now.ToString("G"), FaceModel.Id, FaceModel.IdPersons, FaceModel.FaceId
                )
            );
        }
    }
}
