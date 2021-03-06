﻿using System;
using CG4U.Core.Common.Domain.Messages;
using CG4U.Donate.Domain.Donations.Models;
using Microsoft.Extensions.Logging;

namespace CG4U.Donate.Domain.Donations.Events
{
    public class DesiredUpdatedEvent : Event
    {
        public DesiredModel DesiredModel { get; set; }

        public DesiredUpdatedEvent(ILogger logger, DesiredModel desiredModel)
            : base(logger)
        {
            DesiredModel = desiredModel;
        }

        public override void NotifyEventHandled()
        {
            Logger.LogInformation(
                string.Format(
                    "{0} __ Desired Event Updated successfully. Donation.Id {1}, User.Id {2}",
                    DateTime.Now.ToString("G"), DesiredModel.Donation.Id, DesiredModel.User.IdUserIdentity
                )
            );
        }
    }
}
