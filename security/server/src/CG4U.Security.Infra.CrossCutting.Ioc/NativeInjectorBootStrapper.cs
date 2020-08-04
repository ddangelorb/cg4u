using MediatR;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using CG4U.Core.Common.Domain.Interfaces;
using CG4U.Core.Common.Domain.Handlers;
using CG4U.Security.Domain.ImageProcess.Commands;
using CG4U.Core.Common.Domain.Notifications;
using CG4U.Security.Domain.ImageProcess.Events;
using CG4U.Security.Domain.Persons.Commands;
using CG4U.Security.Domain.Persons.Events;
using CG4U.Security.Domain.ImageProcess.Repository;
using CG4U.Security.Infra.Data.Repository;
using CG4U.Security.Domain.Persons.Repository;
using CG4U.Security.Domain.Configuration.Repository;
using CG4U.Core.Services.Interfaces;
using CG4U.Core.Services.Services;
using CG4U.Security.Services.Interfaces;
using CG4U.Security.Services.Services;
using CG4U.Security.Services.Services.Azure;
using CG4U.Security.Domain.Configuration.Commands;
using CG4U.Security.Domain.Configuration.Events;

namespace CG4U.Security.Infra.CrossCutting.Ioc
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
            services.AddScoped<IAsyncNotificationHandler<AddImageProcessAnalyzeCommand>, ImageProcessCommandHandler>();
            services.AddScoped<IAsyncNotificationHandler<AddImageProcessCommand>, ImageProcessCommandHandler>();
            services.AddScoped<IAsyncNotificationHandler<UpdateImageProcessAnalyzeCommand>, ImageProcessCommandHandler>();
            services.AddScoped<IAsyncNotificationHandler<AddPersonCommand>, PersonCommandHandler>();
            services.AddScoped<IAsyncNotificationHandler<AddPersonFaceCommand>, PersonCommandHandler>();
            services.AddScoped<IAsyncNotificationHandler<AddPersonGroupCommand>, PersonCommandHandler>();
            services.AddScoped<IAsyncNotificationHandler<AddVideoCameraCommand>, ConfigurationCommandHandler>();
            services.AddScoped<IAsyncNotificationHandler<AddAnalyzeRequestCommand>, ConfigurationCommandHandler>();
            services.AddScoped<IAsyncNotificationHandler<AddAlertCommand>, ConfigurationCommandHandler>();
            services.AddScoped<IAsyncNotificationHandler<AddAlertConnectionPersonGroupCommand>, ConfigurationCommandHandler>();
            services.AddScoped<IAsyncNotificationHandler<AddAnalyzeRequestConnectionVideoCameraCommand>, ConfigurationCommandHandler>();

            // Domain - Events
            services.AddScoped<INotificationHandler<DomainNotification>, DomainNotificationHandler>();
            services.AddScoped<INotificationHandler<ImageProcessAddedEvent>, ImageProcessEventHandler>();
            services.AddScoped<INotificationHandler<ImageProcessAnalyzeAddedEvent>, ImageProcessEventHandler>();
            services.AddScoped<INotificationHandler<ImageProcessAnalyzeUpdatedEvent>, ImageProcessEventHandler>();
            services.AddScoped<INotificationHandler<PersonAddedEvent>, PersonEventHandler>();
            services.AddScoped<INotificationHandler<PersonFaceAddedEvent>, PersonEventHandler>();
            services.AddScoped<INotificationHandler<PersonGroupAddedEvent>, PersonEventHandler>();
            services.AddScoped<INotificationHandler<AlertAddedEvent>, ConfigurationEventHandler>();
            services.AddScoped<INotificationHandler<AlertConnectionPersonGroupAddedEvent>, ConfigurationEventHandler>();
            services.AddScoped<INotificationHandler<AnalyzeRequestAddedEvent>, ConfigurationEventHandler>();
            services.AddScoped<INotificationHandler<AnalyzeRequestConnectionVideoCameraAddedEvent>, ConfigurationEventHandler>();
            services.AddScoped<INotificationHandler<VideoCameraAddedEvent>, ConfigurationEventHandler>();

            // Infra - Data
            services.AddScoped<IAlertRepository, AlertRepository>();
            services.AddScoped<IAnalyzeRequestRepository, AnalyzeRequestRepository>();
            services.AddScoped<IImageProcessRepository, ImageProcessRepository>();
            services.AddScoped<IPersonRepository, PersonRepository>();
            services.AddScoped<IVideoCameraRepository, VideoCameraRepository>();

            //Services
            services.AddScoped<IUserAdapter, UserAdapter>();
            services.AddScoped<IComputerVisionAdapter, AzureComputerVisionAdapter>();
            services.AddScoped<IFaceAdapter, AzureFaceAdapter>();
            services.AddScoped<IImageProcessAdapter, ImageProcessAdapter>();
            services.AddScoped<IPersonAdapter, PersonAdapter>();
        }
    }
}
