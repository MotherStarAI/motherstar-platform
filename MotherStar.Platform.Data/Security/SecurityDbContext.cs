using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MotherStar.Platform.Domain;
using MotherStar.Platform.Domain.Security.Models;
using RCommon.Persistence.EFCore;

namespace MotherStar.Platform.Data.Security
{
    public partial class SecurityDbContext : RCommonDbContext
    {
        private readonly IConfiguration _configuration;
        private readonly ILoggerFactory _loggerFactory;

        public SecurityDbContext(DbContextOptions<SeoDbContext> options)
            : base(options)
        {

        }

        public SecurityDbContext(DbContextOptions<SeoDbContext> options, IConfiguration configuration, ILoggerFactory loggerFactory)
            : base(options)
        {
            if (options is null)
            {
                throw new ArgumentNullException(nameof(options));
            }

            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            _loggerFactory = loggerFactory ?? throw new ArgumentNullException(nameof(loggerFactory));
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            

            OnModelCreatingPartial(modelBuilder);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured && _configuration != null)

            {
                var connectionString = _configuration.GetConnectionString(DataStoreNamesConst.LighthouseDb);
                var logger = _loggerFactory.CreateLogger(nameof(SeoDbContext));
                logger.LogDebug("Connection string: {0}", connectionString);
                optionsBuilder.UseNpgsql(connectionString);
                optionsBuilder.UseLoggerFactory(this._loggerFactory);
            }

        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);

        public DbSet<User> Users { get; set; }
    }
}
