using System;
using CG4U.Core.Common.Domain.Interfaces;
using CG4U.Core.Common.Domain.Messages;
using Microsoft.Extensions.Logging;

namespace CG4U.Core.Domain.Users.Events
{
    public class SystemUserAddedEvent : Event
    {
        public string IdUserIdentity { get; set; }
        public int IdSystems { get; set; }

        public SystemUserAddedEvent(ILogger logger, string idUserIdentity, int idSystems)
            : base(logger)
        {
            IdUserIdentity = idUserIdentity;
            IdSystems = idSystems;
        }

        public override void NotifyEventHandled()
        {
            Logger.LogInformation(
                string.Format(
                    "{0} __ System User Added successfully. User.IdUser {1} Systems.Id {2}",
                    DateTime.Now.ToString("G"), IdUserIdentity, IdSystems
                )
            );
        }
    }
}
