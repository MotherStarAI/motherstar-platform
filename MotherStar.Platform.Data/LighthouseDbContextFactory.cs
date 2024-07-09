using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using MotherStar.Platform.Domain;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MotherStar.Platform.Data
{
    /// <summary>
    /// This allows us to use the EF tools from commandline without interrupting logic inside of <see cref="BackOfficeDbContext"/>
    /// </summary>
	public class LighthouseDbContextFactory : IDesignTimeDbContextFactory<LighthouseDbContext>
    {
        public LighthouseDbContext CreateDbContext(string[] args)
        {
            IConfigurationRoot configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .AddUserSecrets<LighthouseDbContext>()
                .Build();

            var builder = new DbContextOptionsBuilder<LighthouseDbContext>();
            var connectionString = configuration.GetConnectionString(DataStoreNamesConst.LighthouseDb);
            builder.UseNpgsql(connectionString);

            return new LighthouseDbContext(builder.Options);
        }
    }
}
