using Microsoft.EntityFrameworkCore;
using Server.Entities;
using Server.Entities.Identity;

namespace Server.DataBase;

public interface IApplicationDbContext
{
    public DbSet<Telemetry> Telementries { get; set; }
    public DbSet<User> Users { get; set; }
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}