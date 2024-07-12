using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Configuration;
using System;
using System.Runtime.CompilerServices;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Swashbuckle.AspNetCore.SwaggerGen;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Versioning;
using System.Text;
using MotherStar.Platform.HttpApi.Authentication;
using MotherStar.Platform.HttpApi.Filters;
using MotherStar.Platform.HttpApi;
using MotherStar.Platform.HttpApi.Swagger;

namespace MotherStar.Platform.HttpApi.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static void AddSeoHttpApi(this IServiceCollection services, IConfiguration configuration)
        {
            //Add API Key Authentication
            services.AddAuthentication(ApiKeyDefaults.ApiKeyAuthSchemeName)
                .AddScheme<ApiKeyAuthOptions, ApiKeyAuthHandler>(ApiKeyDefaults.ApiKeyAuthSchemeName, null);


            // Add the API Key Authentication service
            services.AddSingleton<IApiKeyAuthenticationService, ApiKeyAuthenticationService>();
            services.AddCors(options =>
            {
                options.AddPolicy(SeoHttpApiDefaults.CorsPolicyDefault,
                     builder =>
                     {
                         builder.WithOrigins(configuration.GetSection(SeoHttpApiDefaults.ValidRequestOriginsConfigNameDefault).Get<string[]>()) // Depending on API gateway, we may be able to lock down origin to that IP. Allowing all for now.
                         .AllowAnyHeader()
                         .AllowAnyMethod()
                        .AllowCredentials();
                     });
            });

            // Authorization
            services.AddAuthorization();

            // Caching
            services.AddDistributedMemoryCache();

            // Swagger Stuff
            services.AddTransient<IConfigureOptions<SwaggerGenOptions>, ConfigureSwaggerGenOptions>();

            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen(o =>
            {
                o.OperationFilter<SwaggerDefaultValues>();
            });

            services.AddApiVersioning(options =>
            {
                options.DefaultApiVersion = new ApiVersion(1, 0);
                options.AssumeDefaultVersionWhenUnspecified = true;
                options.ReportApiVersions = true;
                options.ApiVersionReader = new HeaderApiVersionReader("X-Api-Version");
            });

            services.AddVersionedApiExplorer(setup =>
            {
                setup.GroupNameFormat = "'v'VVV";
                setup.SubstituteApiVersionInUrl = true;
            });

            services.AddHealthChecks();
            services.AddControllers(options =>
            {
                options.Filters.Add(typeof(HttpGlobalExceptionFilter)); // Global Exception handling
            }).AddJsonOptions(options => options.JsonSerializerOptions.WriteIndented = true);

            services.AddHttpContextAccessor(); // Allows us to encapsulate the HttpContext
            services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromSeconds(10);
                options.Cookie.HttpOnly = true;
                options.Cookie.IsEssential = true;
            });
        }
    }
}
