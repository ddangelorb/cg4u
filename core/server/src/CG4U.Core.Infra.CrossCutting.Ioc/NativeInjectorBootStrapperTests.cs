using MediatR;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using CG4U.Core.Common.Domain.Interfaces;
using CG4U.Core.Common.Domain.Handlers;
using CG4U.Core.Common.Domain.Notifications;
using CG4U.Core.Domain.Users.Commands;
using CG4U.Core.Domain.Users.Events;
using CG4U.Core.Domain.Users.Repository;
using CG4U.Core.Infra.Data.Repository;
using CG4U.Core.Services.Interfaces;
using CG4U.Core.Services.Services;

namespace CG4U.Core.Infra.CrossCutting.Ioc
{
    public class NativeInjectorBootStrapperTests
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
            services.AddScoped<IAsyncNotificationHandler<AddSystemUserCommand>, UserCommandHandler>();
            services.AddScoped<IAsyncNotificationHandler<AddUserCommand>, UserCommandHandler>();
            services.AddScoped<IAsyncNotificationHandler<EnableUserCommand>, UserCommandHandler>();
            services.AddScoped<IAsyncNotificationHandler<DisableUserCommand>, UserCommandHandler>();
            services.AddScoped<IAsyncNotificationHandler<UpdateUserCommand>, UserCommandHandler>();

            // Domain - Events
            services.AddScoped<INotificationHandler<DomainNotification>, DomainNotificationHandler>();
            services.AddScoped<INotificationHandler<SystemUserAddedEvent>, UserEventHandler>();
            services.AddScoped<INotificationHandler<UserAddedEvent>, UserEventHandler>();
            services.AddScoped<INotificationHandler<UserEnabledEvent>, UserEventHandler>();
            services.AddScoped<INotificationHandler<UserDisabledEvent>, UserEventHandler>();
            services.AddScoped<INotificationHandler<UserUpdatedEvent>, UserEventHandler>();

            // Infra - Data
            services.AddScoped<IUserRepository, UserRepository>();

            //Services
            services.AddScoped<IUserAdapter, UserAdapterTests>();
        }
    }
}
