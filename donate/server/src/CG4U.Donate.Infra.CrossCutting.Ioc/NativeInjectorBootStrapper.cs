using MediatR;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using CG4U.Donate.Domain.Donations.Commands;
using CG4U.Donate.Domain.Donations.Repository;
using CG4U.Donate.Infra.Data.Repository;
using CG4U.Donate.Domain.Donations.Events;
using CG4U.Donate.Domain.Trades.Commands;
using CG4U.Donate.Domain.Trades.Events;
using CG4U.Donate.Domain.Trades.Repository;
using CG4U.Core.Common.Domain.Interfaces;
using CG4U.Core.Common.Domain.Handlers;
using CG4U.Core.Common.Domain.Notifications;
using CG4U.Core.Services.Interfaces;
using CG4U.Core.Services.Services;

namespace CG4U.Donate.Infra.CrossCutting.Ioc
{
    public class NativeInjectorBootStrapper
    {
        public static void RegisterServices(IServiceCollection services)
        {
            // ASPNET
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddSingleton(Mapper.Configuration);
            services.AddScoped<IMapper>(sp => new Mapper(sp.GetRequiredService<IConfigurationProvider>(), sp.GetService));

            // Domain Bus (Mediator)
            services.AddScoped<IMediatorHandler, MediatorHandler>();

            // Domain - Commands
            services.AddScoped<IAsyncNotificationHandler<AddDesiredCommand>, DonationCommandHandler>();
            services.AddScoped<IAsyncNotificationHandler<AddGivenCommand>, DonationCommandHandler>();
            services.AddScoped<IAsyncNotificationHandler<DisableDesiredCommand>, DonationCommandHandler>();
            services.AddScoped<IAsyncNotificationHandler<DisableGivenCommand>, DonationCommandHandler>();
            services.AddScoped<IAsyncNotificationHandler<UpdateDesiredCommand>, DonationCommandHandler>();
            services.AddScoped<IAsyncNotificationHandler<UpdateGivenCommand>, DonationCommandHandler>();
            services.AddScoped<IAsyncNotificationHandler<AddGivenImageCommand>, DonationCommandHandler>();
            services.AddScoped<IAsyncNotificationHandler<AddTradeCommand>, TradeCommandHandler>();
            services.AddScoped<IAsyncNotificationHandler<AddTradeEvaluationCommand>, TradeCommandHandler>();
            services.AddScoped<IAsyncNotificationHandler<AddTradeLocationCommand>, TradeCommandHandler>();
            services.AddScoped<IAsyncNotificationHandler<DisableTradeCommand>, TradeCommandHandler>();
            services.AddScoped<IAsyncNotificationHandler<DisableTradeLocationCommand>, TradeCommandHandler>();
            services.AddScoped<IAsyncNotificationHandler<UpdateTradeCommand>, TradeCommandHandler>();
            services.AddScoped<IAsyncNotificationHandler<UpdateTradeEvaluationCommand>, TradeCommandHandler>();
            services.AddScoped<IAsyncNotificationHandler<UpdateTradeLocationCommand>, TradeCommandHandler>();

            // Domain - Events
            services.AddScoped<INotificationHandler<DomainNotification>, DomainNotificationHandler>();
            services.AddScoped<INotificationHandler<DesiredAddedEvent>, DonationEventHandler>();
            services.AddScoped<INotificationHandler<DesiredDisabledEvent>, DonationEventHandler>();
            services.AddScoped<INotificationHandler<DesiredUpdatedEvent>, DonationEventHandler>();
            services.AddScoped<INotificationHandler<GivenAddedEvent>, DonationEventHandler>();
            services.AddScoped<INotificationHandler<GivenDisabledEvent>, DonationEventHandler>();
            services.AddScoped<INotificationHandler<GivenUpdatedEvent>, DonationEventHandler>();
            services.AddScoped<INotificationHandler<TradeAddedEvent>, TradeEventHandler>();
            services.AddScoped<INotificationHandler<TradeDisabledEvent>, TradeEventHandler>();
            services.AddScoped<INotificationHandler<TradeEvaluationAddedEvent>, TradeEventHandler>();
            services.AddScoped<INotificationHandler<TradeEvaluationUpdatedEvent>, TradeEventHandler>();
            services.AddScoped<INotificationHandler<TradeLocationAddedEvent>, TradeEventHandler>();
            services.AddScoped<INotificationHandler<TradeLocationDisabledEvent>, TradeEventHandler>();
            services.AddScoped<INotificationHandler<TradeLocationUpdatedEvent>, TradeEventHandler>();
            services.AddScoped<INotificationHandler<TradeUpdatedEvent>, TradeEventHandler>();

            // Infra - Data
            services.AddScoped<IDesiredRepository, DesiredRepository>();
            services.AddScoped<IDonationRepository, DonationRepository>();
            services.AddScoped<IGivenRepository, GivenRepository>();
            services.AddScoped<ITradeRepository, TradeRepository>();

            //Services
            services.AddScoped<IUserAdapter, UserAdapter>();
        }
    }
}
