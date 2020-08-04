using System;
using CG4U.Core.Common.Domain.Messages;
using Microsoft.Extensions.Logging;

namespace CG4U.Core.Domain.Users.Events
{
    public class UserEnabledEvent : Event
    {
        public string IdUserIdentiy { get; set; }

        public UserEnabledEvent(ILogger logger, string idUserIdentiy)
            : base(logger)
        {
            IdUserIdentiy = idUserIdentiy;
        }

        public override void NotifyEventHandled()
        {
            Logger.LogInformation(
                string.Format(
                    "{0} __ User Enabled successfully. User.IdUserIdentity {1}",
                    DateTime.Now.ToString("G"), IdUserIdentiy
                )
            );
        }
    }
}
