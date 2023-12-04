using Microsoft.EntityFrameworkCore;
using Server.Entities;

namespace Server.DataBase
{
    public interface IApplicationDbContext
    {
        public DbSet<Telemetry> Telementries { get; set; }
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}
