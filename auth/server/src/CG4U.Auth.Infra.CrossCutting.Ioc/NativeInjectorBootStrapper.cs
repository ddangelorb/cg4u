using AutoMapper;
using CG4U.Core.Common.Domain.Handlers;
using CG4U.Core.Common.Domain.Interfaces;
using CG4U.Core.Common.Domain.Notifications;
using CG4U.Core.Services.Interfaces;
using CG4U.Core.Services.Services;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace CG4U.Auth.Infra.CrossCutting.Ioc
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

            services.AddScoped<INotificationHandler<DomainNotification>, DomainNotificationHandler>();

            //Services
            services.AddTransient<IEmailSender, MessageSender>();
            services.AddScoped<IUserAdapter, UserAdapter>();
        }
    }
}
