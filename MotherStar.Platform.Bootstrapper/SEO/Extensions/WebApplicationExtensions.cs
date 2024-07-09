using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using MotherStar.Platform.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MotherStar.Platform.Bootstrapper.SEO.Extensions
{
    public static class WebApplicationExtensions
    {

        public static void RunEntityFrameworkMigrations(this WebApplication app)
        {
            var scope = app.Services.CreateScope();
            var context = scope.ServiceProvider.GetService<LighthouseDbContext>();
            context.Database.Migrate();
        }
    }
}
