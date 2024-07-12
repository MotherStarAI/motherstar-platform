using FluentValidation;
using Hangfire;
using Hangfire.PostgreSql;
using Hangfire.SqlServer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MotherStar.Platform.Application.SEO;
using MotherStar.Platform.Application.SEO.Lighthouse.Services;
using MotherStar.Platform.Domain;
using RCommon;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;


namespace MotherStar.Platform.Application.SEO.Extensions
{
    public static class IServiceCollectionExtensions
    {


        public static void AddSeoApplicationServices(this IServiceCollection services)
        {
            // Add Services
            services.AddTransient<ILighthouseAppService, LighthouseAppService>();

            // Factory for Lighthouse Report Generator. Doing it this way so we can set generator options later. 
            services.AddTransient<ILighthouseReportGenerator, LighthouseApiReportGenerator>();
            services.AddTransient<Func<ILighthouseReportGenerator>>(x => () => x.GetService<ILighthouseReportGenerator>());
            services.AddTransient<ICommonFactory<ILighthouseReportGenerator>, CommonFactory<ILighthouseReportGenerator>>();

            // AutoMapper Mapping Profiles
            services.AddAutoMapper(x => // Where all of our DTO mapping occurs
            {
                x.AddProfile<SeoAutoMapperProfile>();
            });
        }

        public static void AddSeoBackgroundJobs(this IServiceCollection services, IConfiguration configuration)
        {
            JobStorage.Current = new PostgreSqlStorage(configuration.GetConnectionString(DataStoreNamesConst.LighthouseDb));

            // Add Hangfire services.
            services.AddHangfire(config => config
                    .UseSerilogLogProvider()
                    .SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
                    .UseSimpleAssemblyNameTypeSerializer()
                    .UseRecommendedSerializerSettings()
                    .UsePostgreSqlStorage(configuration.GetConnectionString(DataStoreNamesConst.LighthouseDb), new PostgreSqlStorageOptions
                    {
                        //CommandBatchMaxTimeout = TimeSpan.FromMinutes(5),
                        //SlidingInvisibilityTimeout = TimeSpan.FromMinutes(5),
                        QueuePollInterval = TimeSpan.FromSeconds(10),
                        //UseRecommendedIsolationLevel = true,
                        //DisableGlobalLocks = true
                    }));

            // Add the processing server as IHostedService
            services.AddHangfireServer();
        }



    }
}
