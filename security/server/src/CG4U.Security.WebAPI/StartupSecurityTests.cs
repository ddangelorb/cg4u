using System;
using System.Collections.Generic;
using System.Globalization;
using System.IdentityModel.Tokens.Jwt;
using AutoMapper;
using CG4U.Core.Common.Domain.Models;
using CG4U.Core.Services.Authorization;
using CG4U.Core.Services.Handlers;
using CG4U.Core.Services.InitialSetup;
using CG4U.Security.Infra.CrossCutting.Ioc;
using CG4U.Security.Services.Data;
using CG4U.Security.WebAPI.Hubs;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using NLog.Extensions.Logging;
using NLog.Web;

namespace CG4U.Security.WebAPI
{
    public class StartupSecurityTests
    {
        public StartupSecurityTests(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            var tokenConfigurations = new TokenDescriptor();
            new ConfigureFromConfigurationOptions<TokenDescriptor>(
                    Configuration.GetSection("JwtTokenOptions"))
                .Configure(tokenConfigurations);
            services.AddSingleton(tokenConfigurations);

            services.AddOptions();
            services.Configure<DbConnection>(Configuration.GetSection("ConnectionStrings"));
            services.Configure<EmailSender>(Configuration.GetSection("EmailSender"));
            services.Configure<TokenDescriptor>(Configuration.GetSection("JwtTokenOptions"));
            services.Configure<UserApi>(Configuration.GetSection("UserApi"));
            services.Configure<AzureApiData>(Configuration.GetSection("AzureApiData"));
            services.AddDbContext<ApplicationDbContext>();

            services.AddIdentity<IdentityUser, IdentityRole>(options =>
            {
                options.User.RequireUniqueEmail = true;
            })
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear(); // => remove default claims
            services
                .AddAuthentication(options =>
                {
                    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;

                })
                .AddJwtBearer(cfg =>
                {
                    cfg.RequireHttpsMetadata = false;
                    cfg.SaveToken = true;

                    var paramsValidation = cfg.TokenValidationParameters;
                    paramsValidation.ValidateIssuerSigningKey = true;
                    paramsValidation.IssuerSigningKey = SigningCredentialsConfiguration.Key;
                    paramsValidation.ValidateIssuer = true;
                    paramsValidation.ValidIssuer = tokenConfigurations.Issuer;
                    paramsValidation.ValidateAudience = true;
                    paramsValidation.ValidAudience = tokenConfigurations.Audience;
                    paramsValidation.ValidateLifetime = true; //validate the expiration and not before values in the token
                    paramsValidation.ClockSkew = TimeSpan.Zero; //0 minutes tolerance for the expiration date
                });

            services.AddAuthorization(options =>
            {
                options.AddPolicy("Admin", policy => policy.RequireRole("Admin"));
                options.AddPolicy("UserSecurityAdmin", policy => policy.RequireRole("UserSecurityAdmin"));
                options.AddPolicy("UserSecurity", policy => policy.RequireRole("UserSecurity"));
                options.AddPolicy("Bearer", new AuthorizationPolicyBuilder()
                    .AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme)
                    .RequireAuthenticatedUser().Build());
            });

            services.AddMvc().
                    AddViewLocalization(
                        LanguageViewLocationExpanderFormat.Suffix,
                        opts => { opts.ResourcesPath = "Resources"; }
                       ).AddDataAnnotationsLocalization();

            //Multi-languages
            //Source-files are found at Resource folder '/Controllers, ViewModels, Views, etc
            //The resource files were generate by 'resgen' command from Mono, based on a txt-source file
            //  i.e.: $ resgen "BaseController.pt-BR.txt" "BaseController.pt-BR.resx"
            services.AddLocalization(opts => { opts.ResourcesPath = "Resources"; });
            services.Configure<RequestLocalizationOptions>(
                opts =>
                {
                    var supportedCultures = new List<CultureInfo>
                    {
                        new CultureInfo("pt"),
                        new CultureInfo("pt-BR"),
                        new CultureInfo("en"),
                        new CultureInfo("en-GB"),
                        new CultureInfo("en-US"),
                        new CultureInfo("es")/*,
                        new CultureInfo("fr"),
                        new CultureInfo("it"),
                        new CultureInfo("de")*/
                    };

                    opts.DefaultRequestCulture = new RequestCulture("pt-BR");
                    opts.SupportedCultures = supportedCultures;
                    opts.SupportedUICultures = supportedCultures;
                });

            services.AddAutoMapper();
            services.AddMediatR(typeof(Startup));
            services.AddCors();
            services.AddLogging();
            services.AddSignalR();

            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            NativeInjectorBootStrapperTests.RegisterServices(services);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            app.UseDeveloperExceptionPage();
            app.UseCors(builder =>
            {
                builder.AllowAnyHeader();
                builder.AllowAnyMethod();
                builder.AllowCredentials();
                builder.AllowAnyOrigin();
            });

            env.ConfigureNLog("nlog.config");
            loggerFactory.AddNLog();
            app.AddNLogWeb();

            app.UseSignalR(routes =>
            {
                routes.MapHub<AlertHub>("/alertHub");
            });
            app.UseAuthentication(); //
            app.UseMiddleware(typeof(ErrorHandlerMiddleware));
            app.UseMvc();
            app.UseStaticFiles();

            //Multi-languages
            var optLanguages = app.ApplicationServices.GetService<IOptions<RequestLocalizationOptions>>();
            app.UseRequestLocalization(optLanguages.Value);

            app.UseForwardedHeaders(new ForwardedHeadersOptions
            {
                ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
            });
        }
    }
}
