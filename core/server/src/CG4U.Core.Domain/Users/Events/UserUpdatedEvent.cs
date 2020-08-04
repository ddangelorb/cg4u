using System;
using CG4U.Core.Common.Domain.Messages;
using CG4U.Core.Domain.Users.Models;
using Microsoft.Extensions.Logging;

namespace CG4U.Core.Domain.Users.Events
{
    public class UserUpdatedEvent : Event
    {
        public UserModel UserModel { get; set; }

        public UserUpdatedEvent(ILogger logger, UserModel userModel)
            : base(logger)
        {
            UserModel = userModel;
        }

        public override void NotifyEventHandled()
        {
            Logger.LogInformation(
                string.Format(
                    "{0} __ User Event Updated successfully. User.IdUserIdentity {1}, User.Email {2}",
                    DateTime.Now.ToString("G"), UserModel.IdUserIdentity, UserModel.Email
                )
            );
        }
    }
}
