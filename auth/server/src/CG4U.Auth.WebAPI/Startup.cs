﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Localization;
using MediatR;
using NLog.Web;
using Microsoft.Extensions.Logging;
using NLog.Extensions.Logging;
using AutoMapper;
using CG4U.Core.Common.Domain.Models;
using CG4U.Core.Services.ViewModels;
using CG4U.Core.Common.Domain.Interfaces;
using CG4U.Core.Services.Authorization;
using CG4U.Core.Services.InitialSetup;
using CG4U.Auth.Infra.CrossCutting.Ioc;
using CG4U.Core.Services.Handlers;

namespace CG4U.Auth.WebAPI
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
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

            services.Configure<DbConnection>(Configuration.GetSection("ConnectionStrings"));
            services.Configure<UserApi>(Configuration.GetSection("UserApi"));
            services.Configure<EmailSender>(Configuration.GetSection("EmailSender"));
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

            services.AddMvc();

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
                    // Formatting numbers, dates, etc.
                    opts.SupportedCultures = supportedCultures;
                    // UI strings that we have localized.
                    opts.SupportedUICultures = supportedCultures;
                });

            services.Configure<TokenDescriptor>(Configuration.GetSection("JwtTokenOptions"));
            services.AddMediatR(typeof(Startup));
            services.AddCors();
            services.AddLogging();
            services.AddAutoMapper();

            NativeInjectorBootStrapper.RegisterServices(services);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ApplicationDbContext dbContext, ILoggerFactory loggerFactory, UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager, IOptions<UserApi> userApiOption, IOptions<TokenDescriptor> tokenDescriptor) 
        {
            if (env.IsDevelopment())
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
            }
            else if (env.IsProduction())
            {
                //TODO: Configure that properly
                app.UseCors(
                    optCors => optCors.WithOrigins("http://localhost:4200/").AllowAnyMethod());

                loggerFactory.AddAzureWebAppDiagnostics();
                // loggerFactory.AddApplicationInsights(app.ApplicationServices);
            }

            app.UseAuthentication();
            app.UseMiddleware(typeof(ErrorHandlerMiddleware));
            app.UseMvc();
            app.UseStaticFiles();
            dbContext.Database.EnsureCreated();

            //setup data initializer
            ILogger logger = loggerFactory.CreateLogger(GetType().Namespace);
            var usersSeed = new List<UserViewModel>();
            Configuration.GetSection("Logins").Bind(usersSeed);
            DataInitializer.SeedData(logger, userManager, roleManager, usersSeed, userApiOption, tokenDescriptor);

            //Multi-languages
            var optLanguages = app.ApplicationServices.GetService<IOptions<RequestLocalizationOptions>>();
            app.UseRequestLocalization(optLanguages.Value);     
        }
    }
}
