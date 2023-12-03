using Microsoft.EntityFrameworkCore;
using Server.Entities;

namespace Server.DataBase
{
    public class ApplicationDbContext : DbContext, IApplicationDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<Telemetry> Telementries { get; set; }

        public static ApplicationDbContext CreateDbContext(IConfiguration configuration)
        {
            var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
            optionsBuilder.UseNpgsql(configuration.GetConnectionString("DbConnect"));

            return new ApplicationDbContext(optionsBuilder.Options);
        }

        public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            return await base.SaveChangesAsync(cancellationToken);
        }
    }
}
