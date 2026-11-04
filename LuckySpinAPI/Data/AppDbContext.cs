using Microsoft.EntityFrameworkCore;
using LuckySpinAPI.Models;

namespace LuckySpinAPI.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<GameLog> GameLogs { get; set; }
    }
}
