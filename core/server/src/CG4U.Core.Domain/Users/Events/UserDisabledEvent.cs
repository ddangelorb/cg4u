using System;
using CG4U.Core.Common.Domain.Messages;
using Microsoft.Extensions.Logging;

namespace CG4U.Core.Domain.Users.Events
{
    public class UserDisabledEvent : Event
    {
        public int IdUser { get; set; }

        public UserDisabledEvent(ILogger logger, int idUser)
            : base(logger)
        {
            IdUser = idUser;
        }

        public override void NotifyEventHandled()
        {
            Logger.LogInformation(
                string.Format(
                    "{0} __ User Disabled successfully. User.IdUser {1}",
                    DateTime.Now.ToString("G"), IdUser
                )
            );
        }
    }
}
