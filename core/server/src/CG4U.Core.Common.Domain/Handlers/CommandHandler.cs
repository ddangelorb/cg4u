using System.Collections.Generic;
using CG4U.Core.Common.Domain.Interfaces;
using CG4U.Core.Common.Domain.Models;
using CG4U.Core.Common.Domain.Notifications;
using FluentValidation.Results;
using Microsoft.Extensions.Logging;

namespace CG4U.Core.Common.Domain.Handlers
{
    public abstract class CommandHandler
    {
        private readonly IMediatorHandler _mediator;
        private readonly DomainNotificationHandler _notifications;
        protected readonly ILogger<CommandHandler> _logger;

        protected CommandHandler(IMediatorHandler mediator, DomainNotificationHandler notifications, ILogger<CommandHandler> logger)
        {
            _mediator = mediator;
            _notifications = notifications;
            _logger = logger;
        }

        protected void NotifyErrorValidations(string messageType, string messageError)
        {
            var vr = new ValidationResult();
            vr.Errors.Add(new ValidationFailure(messageType, messageError));
            NotifyErrorValidations(vr);
        }

        protected void NotifyErrorValidations(ValidationResult validationResult)
        {
            foreach (var error in validationResult.Errors)
            {
                _mediator.PublishEvent(new DomainNotification(_logger, error.PropertyName, error.ErrorMessage));
            }
        }

        protected bool IsEntityFound<T>(Entity<T> entity, string messageType, string messageError) where T : Entity<T>
        {
            if (entity == null)
            {
                NotifyErrorValidations(messageType, messageError);
                return false;
            }
            return true;
        }

        protected bool IsEntityValid<T>(Entity<T> entity) where T : Entity<T>
        {
            if (entity.IsValid()) return true;

            NotifyErrorValidations(entity.ValidationResult);
            return false;
        }

        protected bool Commit()
        {
            if (!_notifications.HasNotifications()) return true;

            NotifyErrorValidations("Commit", "Error doing Commit.");
            return false;
        }

        protected bool IsUsersValid(User userLoggedIn, User userEntity, params User[] allUsersCommand)
        {
            var users = new List<User>();
            users.Add(userLoggedIn);
            users.Add(userEntity);
            users.AddRange(allUsersCommand);
            if (!IsUsersActive(users.ToArray())) return false;

            if (!IsUserCanModifyEntity(userLoggedIn, userEntity)) return false;

            return true;
        }

        protected bool IsUserCanModifyEntity(User userLoggedIn, User userEntity)
        {
            if (!(userLoggedIn.Roles.Contains(UserRoles.Admin)
               || userLoggedIn.Id == userEntity.Id))
            {
                NotifyErrorValidations("User", "User.CannotModifyEntity");
                return false;
            }

            return true;
        }

        private bool IsUsersActive(params User[] listUser)
        {
            foreach (var user in listUser)
            {
                if (user == null || user.IdentityUser == null)
                {
                    NotifyErrorValidations("User", "User.NotInformed");
                    return false;
                }

                if (user.Active == 0
                    || !user.IdentityUser.EmailConfirmed)
                {
                    NotifyErrorValidations("User", "User.IsNotValid");
                    return false;
                }
            }

            return true;
        }
    }
}
