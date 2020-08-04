using System;
using CG4U.Core.Common.Domain.Messages;
using CG4U.Donate.Domain.Donations.Models;
using Microsoft.Extensions.Logging;

namespace CG4U.Donate.Domain.Donations.Events
{
    public class GivenImageAddedEvent : Event
    {
        public GivenModel GivenModel { get; set; }

        public GivenImageAddedEvent(ILogger logger, GivenModel givenModel)
            : base(logger)
        {
            GivenModel = givenModel;
        }

        public override void NotifyEventHandled()
        {
            Logger.LogInformation(
                string.Format(
                    "{0} __ Given Image Added successfully. Donation.Id {1}, User.Id {2}",
                    DateTime.Now.ToString("G"), GivenModel.Donation.Id, GivenModel.User.IdUserIdentity
                )
            );
        }
    }
}
