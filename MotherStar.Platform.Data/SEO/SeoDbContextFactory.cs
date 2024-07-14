using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using MotherStar.Platform.Domain;

namespace MotherStar.Platform.Data.SEO
{
    public class SeoDbContextFactory : IDesignTimeDbContextFactory<SeoDbContext>
    {
        public SeoDbContext CreateDbContext(string[] args)
        {
            IConfigurationRoot configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .AddUserSecrets<SeoDbContext>()
                .Build();

            var builder = new DbContextOptionsBuilder<SeoDbContext>();
            var connectionString = configuration.GetConnectionString(DataStoreNamesConst.SeoDb);
            builder.UseNpgsql(connectionString);

            return new SeoDbContext(builder.Options);
        }
    }
}
