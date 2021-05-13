using IdentityServer4.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Users.Data;
using Users.Data.Configuration;
using Users.Data.Models;
using Users.Service.Extensions;
using Users.Service.Profile;
using Users.Service.RequestValidators;

namespace Users.API.Extensions
{
    public static class ServiceCollectionExtension
    {
        public static IServiceCollection AddDbContext(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<ApplicationUsersDbContext>(options =>
                    options.UseSqlServer(
                        configuration.GetConnectionString("DefaultConnection"),
                        sqlServerOptionsAction: sqlOptions =>
                        {
                            sqlOptions.MigrationsAssembly(Assembly.GetAssembly(typeof(ApplicationUsersDbContext)).GetName().Name);
                            sqlOptions.EnableRetryOnFailure(maxRetryCount: 15, maxRetryDelay: TimeSpan.FromSeconds(30), errorNumbersToAdd: null);
                        })
                    );

            return services;
        }

        public static IServiceCollection SetupDevelopmentCorsPolicies(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddCors(options =>
            {
                options.AddDefaultPolicy(
                    builder =>
                    {
                        builder.WithOrigins(
                            AppSettingsHelper.DESKTOP_APP_URI(configuration),
                            AppSettingsHelper.WEBSITE_URI(configuration)
                        )
                        .AllowCredentials()
                        .AllowAnyMethod()
                        .AllowAnyHeader();
                    });
            });

            services.AddSingleton<ICorsPolicyService>((container) =>
            {
                var logger = container.GetRequiredService<ILogger<DefaultCorsPolicyService>>();
                return new DefaultCorsPolicyService(logger)
                {
                    AllowedOrigins = { 
                        AppSettingsHelper.DESKTOP_APP_URI(configuration),
                        AppSettingsHelper.WEBSITE_URI(configuration)
                    }
                };
            });
            return services;
        }

        public static IServiceCollection SetupIdentityServer(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<ApplicationUsersDbContext>()
                .AddDefaultTokenProviders();

            services.ConfigureApplicationCookie(config =>
            {
                config.Cookie.Name = "IdentityServer.Cookie";
                config.LoginPath = "/api/redirect/login";
                config.LogoutPath = "/api/logout";
                config.AccessDeniedPath = "/api/redirect/login";
                config.ClaimsIssuer = AppSettingsHelper.AUTH_ISSUER(configuration);
            });

            services.AddIdentityServer(options =>
            {
                options.IssuerUri = AppSettingsHelper.AUTH_ISSUER(configuration);
                options.UserInteraction.ErrorUrl = "/api/redirect/login";
                options.Events.RaiseErrorEvents = true;
                options.Events.RaiseInformationEvents = true;
                options.Events.RaiseFailureEvents = true;
                options.Events.RaiseSuccessEvents = true;
                options.EmitStaticAudienceClaim = true;
                options.Authentication.CookieLifetime = TimeSpan.FromHours(1);
            })
            .AddDeveloperSigningCredential()
            .AddAspNetIdentity<ApplicationUser>()
            .AddConfigurationStore(options =>
            {
                options.ConfigureDbContext = builder => builder.UseSqlServer(
                    AppSettingsHelper.DATABASE_CONNECTION(configuration),
                    sqlServerOptionsAction: sqlOptions =>
                    {
                        sqlOptions.MigrationsAssembly(Assembly.GetAssembly(typeof(ApplicationUsersDbContext)).GetName().Name);
                        sqlOptions.EnableRetryOnFailure(maxRetryCount: 15, maxRetryDelay: TimeSpan.FromSeconds(30), errorNumbersToAdd: null);
                    });
            })
            .AddOperationalStore(options =>
            {
                options.ConfigureDbContext = builder => builder.UseSqlServer(
                    AppSettingsHelper.DATABASE_CONNECTION(configuration),
                    sqlServerOptionsAction: sqlOptions =>
                    {
                        sqlOptions.MigrationsAssembly(Assembly.GetAssembly(typeof(ApplicationUsersDbContext)).GetName().Name);
                        sqlOptions.EnableRetryOnFailure(maxRetryCount: 15, maxRetryDelay: TimeSpan.FromSeconds(30), errorNumbersToAdd: null);
                    });
            })
            .AddExtensionGrantValidator<TokenExchangeExtensionGrantValidator>()
            .AddCustomAuthorizeRequestValidator<CustomAuthorizeRequestValidator>()
            .Services.AddTransient<IProfileService, ProfileService>();
            return services;
        }
    }
}
