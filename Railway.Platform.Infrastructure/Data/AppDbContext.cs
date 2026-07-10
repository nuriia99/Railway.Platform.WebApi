using Microsoft.EntityFrameworkCore;
using Railway.Platform.Domain.Entities;

namespace Railway.Platform.Infrastructure.Data
{
    public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
    {
        public DbSet<User> Users => Set<User>();
    }
}
