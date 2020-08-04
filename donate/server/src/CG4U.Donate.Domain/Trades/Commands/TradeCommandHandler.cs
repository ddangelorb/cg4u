using System.Threading.Tasks;
using AutoMapper;
using CG4U.Core.Common.Domain.Handlers;
using CG4U.Core.Common.Domain.Interfaces;
using CG4U.Core.Common.Domain.Models;
using CG4U.Core.Common.Domain.Notifications;
using CG4U.Donate.Domain.Common;
using CG4U.Donate.Domain.Trades.Events;
using CG4U.Donate.Domain.Trades.Models;
using CG4U.Donate.Domain.Trades.Repository;
using MediatR;
using Microsoft.Extensions.Logging;

namespace CG4U.Donate.Domain.Trades.Commands
{
    public class TradeCommandHandler : CommandHandler,
        IAsyncNotificationHandler<AddTradeCommand>,
        IAsyncNotificationHandler<AddTradeEvaluationCommand>,
        IAsyncNotificationHandler<AddTradeLocationCommand>,
        IAsyncNotificationHandler<DisableTradeCommand>,
        IAsyncNotificationHandler<DisableTradeLocationCommand>,
        IAsyncNotificationHandler<UpdateTradeCommand>,
        IAsyncNotificationHandler<UpdateTradeEvaluationCommand>,
        IAsyncNotificationHandler<UpdateTradeLocationCommand>
    {
        private readonly IMediatorHandler _mediator;
        private readonly ITradeRepository _tradeRepository;

        public TradeCommandHandler(IMediatorHandler mediator,
                                   ITradeRepository tradeRepository,
                                   ILogger<TradeCommandHandler> logger,
                                   INotificationHandler<DomainNotification> notifications)
            : base(mediator, (DomainNotificationHandler)notifications, logger)
        {
            _mediator = mediator;
            _tradeRepository = tradeRepository;
        }

        public async Task Handle(AddTradeCommand notification)
        {
            var entityCurrent = Mapper.Map<TradeModel, Trade>(notification.TradeModel);
            if (!IsEntityValid(entityCurrent)) return;

            var userEntity = GetUserEntityTrade(notification.UserLoggedIn, entityCurrent.UserGet, entityCurrent.UserLet);
            if (userEntity == null) return;
            if (!IsUsersValid(notification.UserLoggedIn, userEntity, entityCurrent.UserGet, entityCurrent.UserLet)) 
                return;

            await _tradeRepository.AddAsync(entityCurrent);

            if (Commit())
                await _mediator.PublishEvent(new TradeAddedEvent(_logger, Mapper.Map<TradeModel>(notification.TradeModel)));
        }

        public async Task Handle(AddTradeEvaluationCommand notification)
        {
            var entityCurrentNotification = Mapper.Map<TradeEvaluationModel, Evaluation>(notification.TradeEvaluationModel);
            if (!IsEntityValid(entityCurrentNotification)) return;

            var entityCurrentDB = await _tradeRepository.GetByIdAsync(notification.TradeEvaluationModel.IdParent);
            if (!IsEntityFound(entityCurrentDB, notification.MessageType, "Trade.NotFound")) return;

            var userEntity = GetUserEntityTrade(notification.UserLoggedIn, entityCurrentDB.UserGet, entityCurrentDB.UserLet);
            if (userEntity == null) return;
            if (!IsUserCanModifyEntity(notification.UserLoggedIn, userEntity)) return; 

            await _tradeRepository.AddEvaluationAsync(entityCurrentDB.Id, entityCurrentNotification);

            if (Commit())
                await _mediator.PublishEvent(new TradeEvaluationAddedEvent(_logger, Mapper.Map<TradeEvaluationModel>(notification.TradeEvaluationModel)));
        }

        public async Task Handle(AddTradeLocationCommand notification)
        {
            var entityCurrentNotification = Mapper.Map<TradeLocationModel, Location>(notification.TradeLocationModel);
            if (!IsEntityValid(entityCurrentNotification)) return;

            var entityCurrentDB = await _tradeRepository.GetByIdAsync(notification.TradeLocationModel.IdParent);
            if (!IsEntityFound(entityCurrentDB, notification.MessageType, "Trade.NotFound")) return;

            var userEntity = GetUserEntityTrade(notification.UserLoggedIn, entityCurrentDB.UserGet, entityCurrentDB.UserLet);
            if (userEntity == null) return;
            if (!IsUserCanModifyEntity(notification.UserLoggedIn, userEntity)) return; 

            await _tradeRepository.AddLocationAsync(entityCurrentDB.Id, entityCurrentNotification);

            if (Commit())
                await _mediator.PublishEvent(new TradeLocationAddedEvent(_logger, Mapper.Map<TradeLocationModel>(notification.TradeLocationModel)));
        }

        public async Task Handle(DisableTradeCommand notification)
        {
            var entityCurrent = await _tradeRepository.GetByIdAsync(notification.IdTrades);

            if (!IsEntityFound(entityCurrent, notification.MessageType, "Trade.NotFound")) return;

            var userEntity = GetUserEntityTrade(notification.UserLoggedIn, entityCurrent.UserGet, entityCurrent.UserLet);
            if (userEntity == null) return;
            if (!IsUserCanModifyEntity(notification.UserLoggedIn, userEntity)) return; 

            await _tradeRepository.DisableAsync(entityCurrent.Id);

            if (Commit())
                await _mediator.PublishEvent(new TradeDisabledEvent(_logger, notification.IdTrades));
        }

        public async Task Handle(DisableTradeLocationCommand notification)
        {
            var entityCurrent = await _tradeRepository.GetLocationByIdAsync(notification.IdTradeLocations);
            if (!IsEntityFound(entityCurrent, notification.MessageType, "TradeLocation.NotFound")) return;

            var entityCurrentTrade = await _tradeRepository.GetByIdAsync((int)entityCurrent.IdParent);
            if (!IsEntityFound(entityCurrentTrade, notification.MessageType, "Trade.NotFound")) return;

            var userEntity = GetUserEntityTrade(notification.UserLoggedIn, entityCurrentTrade.UserGet, entityCurrentTrade.UserLet);
            if (userEntity == null) return;
            if (!IsUserCanModifyEntity(notification.UserLoggedIn, userEntity)) return; 

            await _tradeRepository.DisableLocationAsync(entityCurrentTrade.Id, entityCurrent.Id);

            if (Commit())
                await _mediator.PublishEvent(new TradeLocationDisabledEvent(_logger, notification.IdTradeLocations));
        }

        public async Task Handle(UpdateTradeCommand notification)
        {
            var entityCurrent = Mapper.Map<TradeModel, Trade>(notification.TradeModel);
            if (notification.TradeModel.Id == 0)
            {
                NotifyErrorValidations("Trade", "Trade.IdTrades.GreaterThanZero");
                return;
            }
            if (!IsEntityValid(entityCurrent)) return;

            var entityCurrentDB = await _tradeRepository.GetByIdAsync(notification.TradeModel.Id);
            if (!IsEntityFound(entityCurrentDB, notification.MessageType, "Trade.NotFound")) return;

            var userEntity = GetUserEntityTrade(notification.UserLoggedIn, entityCurrent.UserGet, entityCurrent.UserLet);
            if (userEntity == null) return;
            if (!IsUsersValid(notification.UserLoggedIn, userEntity, entityCurrent.UserGet, entityCurrent.UserGet))
                return;

            await _tradeRepository.UpdateAsync(entityCurrent);

            if (Commit())
                await _mediator.PublishEvent(new TradeUpdatedEvent(_logger, Mapper.Map<TradeModel>(notification.TradeModel)));
        }

        public async Task Handle(UpdateTradeEvaluationCommand notification)
        {
            var entityCurrentNotification = Mapper.Map<TradeEvaluationModel, Evaluation>(notification.TradeEvaluationModel);
            if (notification.TradeEvaluationModel.Id == 0)
            {
                NotifyErrorValidations("TradeEvaluation", "Evaluation.IdEvaluation.GreaterThanZero");
                return;
            }
            if (!IsEntityValid(entityCurrentNotification)) return;

            var entityCurrentDB = await _tradeRepository.GetByIdAsync(notification.TradeEvaluationModel.IdParent);
            if (!IsEntityFound(entityCurrentDB, notification.MessageType, "Trade.NotFound")) return;

            var userEntity = GetUserEntityTrade(notification.UserLoggedIn, entityCurrentDB.UserGet, entityCurrentDB.UserLet);
            if (userEntity == null) return;
            if (!IsUserCanModifyEntity(notification.UserLoggedIn, userEntity)) return; 

            await _tradeRepository.UpdateEvaluationAsync(entityCurrentDB.Id, entityCurrentNotification);

            if (Commit())
                await _mediator.PublishEvent(new TradeEvaluationUpdatedEvent(_logger, Mapper.Map<TradeEvaluationModel>(notification.TradeEvaluationModel)));
        }

        public async Task Handle(UpdateTradeLocationCommand notification)
        {
            var entityCurrentNotification = Mapper.Map<TradeLocationModel, Location>(notification.TradeLocationModel);
            if (notification.TradeLocationModel.Id == 0)
            {
                NotifyErrorValidations("TradeLocation", "Location.IdLocation.GreaterThanZero");
                return;
            }
            if (!IsEntityValid(entityCurrentNotification)) return;

            var entityCurrentDB = await _tradeRepository.GetByIdAsync(notification.TradeLocationModel.IdParent);
            if (!IsEntityFound(entityCurrentDB, notification.MessageType, "Trade.NotFound")) return;

            var userEntity = GetUserEntityTrade(notification.UserLoggedIn, entityCurrentDB.UserGet, entityCurrentDB.UserLet);
            if (userEntity == null) return;
            if (!IsUserCanModifyEntity(notification.UserLoggedIn, userEntity)) return; 

            await _tradeRepository.UpdateLocationAsync(entityCurrentDB.Id, entityCurrentNotification);

            if (Commit())
                await _mediator.PublishEvent(new TradeLocationUpdatedEvent(_logger, Mapper.Map<TradeLocationModel>(notification.TradeLocationModel)));
        }

        private User GetUserEntityTrade(User userLoggedIn, User userGet, User userLet)
        {
            if (userLoggedIn.Id == userGet.Id) return userGet;

            if (userLoggedIn.Id == userLet.Id) return userLet;

            NotifyErrorValidations("Trade", "Trade.UserEntityNotFound");
            return null;
        }
    }
}
