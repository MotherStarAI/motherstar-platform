using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using MotherStar.Platform.Data;
using MotherStar.Platform.Data.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MotherStar.Platform.Bootstrapper.SEO.Extensions
{
    public static class WebApplicationExtensions
    {

        public static async Task RunSeoMigrations(this WebApplication app)
        {
            var scope = app.Services.CreateScope();
            var seoDbContext = scope.ServiceProvider.GetService<SeoDbContext>();
            var securityDbContext = scope.ServiceProvider.GetService<SecurityDbContext>();
            seoDbContext.Database.Migrate();
            securityDbContext.Database.Migrate();
            await Task.CompletedTask;
        }
    }
}
