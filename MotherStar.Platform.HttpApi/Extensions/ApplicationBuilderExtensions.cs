using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Mvc.ApiExplorer;

namespace MotherStar.Platform.HttpApi.Extensions
{
    internal static class ApplicationBuilderExtensions
    {

        public static IApplicationBuilder UseMotherStarApp(this IApplicationBuilder app, IConfiguration configuration, IWebHostEnvironment env)
        {
            if (env.IsDevelopment() || env.IsStaging())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();

                app.UseSwaggerUI(o =>
                {
                    var webApp = app as WebApplication;
                    var apiVersionDescriptionProvider = webApp.Services.GetRequiredService<IApiVersionDescriptionProvider>();

                    foreach (var description in apiVersionDescriptionProvider.ApiVersionDescriptions)
                    {
                        o.SwaggerEndpoint($"/swagger/{description.GroupName}/swagger.json",
                            $"MotherStar Api - {description.GroupName.ToUpper()}");
                    }
                });
            }

            app.UseHttpsRedirection();

            app.UseRouting();
            app.UseCors(HttpApiDefaults.CorsPolicyDefault);
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseSession();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapHealthChecks("/health");
            });
            return app;
        }

    }
}
