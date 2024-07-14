using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using MotherStar.Platform.Domain;

namespace MotherStar.Platform.Data.Security
{
    public class SecurityDbContextFactory : IDesignTimeDbContextFactory<SecurityDbContext>
    {
        public SecurityDbContext CreateDbContext(string[] args)
        {
            IConfigurationRoot configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .AddUserSecrets<SecurityDbContext>()
                .Build();

            var builder = new DbContextOptionsBuilder<SecurityDbContext>();
            var connectionString = configuration.GetConnectionString(DataStoreNamesConst.SecurityDb);
            builder.UseNpgsql(connectionString);

            return new SecurityDbContext(builder.Options);
        }
    }
}
