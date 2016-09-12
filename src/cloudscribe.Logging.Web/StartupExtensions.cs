﻿using cloudscribe.Logging.Web;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.Extensions.FileProviders;
using System.Reflection;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class StartupExtensions
    {
        public static IServiceCollection AddCloudscribeLogging(this IServiceCollection services)
        {
            services.AddScoped<LogManager>();
            return services;
        }

        public static RazorViewEngineOptions AddEmbeddedViewsForCloudscribeLogging(this RazorViewEngineOptions options)
        {
            options.FileProviders.Add(new EmbeddedFileProvider(
                    typeof(LogManager).GetTypeInfo().Assembly,
                    "cloudscribe.Logging.Web"
                ));

            return options;
        }

        public static AuthorizationOptions AddCloudscribeLoggingDefaultPolicy(this AuthorizationOptions options)
        {
            options.AddPolicy(
                    "SystemLogPolicy",
                    authBuilder =>
                    {
                        authBuilder.RequireRole("ServerAdmins");
                    });

            return options;
        }

    }
}
