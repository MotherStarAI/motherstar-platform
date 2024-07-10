using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using RCommon;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using RCommon.Persistence;
using RCommon.Mediator.Subscribers;
using System.Reflection;
using Serilog;
using System.IO;
using FluentValidation.AspNetCore;
using Microsoft.IdentityModel.Tokens;
using Hangfire.PostgreSql;
using Hangfire;
using MotherStar.Platform.HttpApi.Extensions;
using MotherStar.Platform.Bootstrapper;
using MotherStar.Platform.Application.Extensions;
using MotherStar.Platform.Bootstrapper.SEO.Extensions;


try
{
    // Initialize the Application Builders, Configuration, and Environment variables
    var builder = WebApplication.CreateBuilder(args);

    ConfigurationManager configuration = builder.Configuration;
    IWebHostEnvironment environment = builder.Environment;

    // Manage Secrets
    if (environment.IsDevelopment())
    {
        builder.Configuration.AddUserSecrets<Program>();
    }

    // Configure RCommon for most infrastructure services including persistence, event handling, and mediator request pipeline
    builder.AddRCommonServices();

    // Add Lighthouse Application services. CAN be reused for unit testing
    builder.Services.AddLighthouseApplicationServices();

    // Add Lighthouse Background jobs. CANNOT be reused for unit testing
    builder.Services.AddLighthouseBackgroundJobs(configuration);

    // Add Lighthouse HttpApi specific services. CANNOT be reused for unit testing.
    builder.Services.AddLighthouseHttpApi(configuration);

    // Configure Logger
    builder.Services.AddLogging();
    Log.Logger = SerilogBootstrapper.BuildLoggerConfig(configuration, environment.EnvironmentName)
                                    .CreateBootstrapLogger();
    builder.Host.UseSerilog(Log.Logger);

    // Build the application
    var app = builder.Build();

    Log.Information("Starting up");
    app.UseLighthouse(configuration, environment);
    app.UseSerilogRequestLogging();

    //Start EF Migrations at the end before we run the application
    app.RunEntityFrameworkMigrations();

    app.Run();
}
catch (Exception ex)
{
    // EF Migrations "StopTheHostException" (gracefully) Work around: https://github.com/dotnet/runtime/issues/60600
    string type = ex.GetType().Name;
    if (type.Equals("StopTheHostException", StringComparison.Ordinal))
    {
        throw;
    }
    // In case exception happens prior to logging bootstrapped.This should output to K8s console.
    Console.WriteLine("A Fatal exception occured while launching application: {0}", ex);
}
finally
{
    Log.Information("Shut down complete");
    Log.CloseAndFlush();
}
