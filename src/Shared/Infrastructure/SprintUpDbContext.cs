using Microsoft.EntityFrameworkCore;
using SprintUp.Modules.UserManagement.Domain.Entities;

namespace SprintUp.Shared.Infrastructure
{
    public class SprintUpDbContext : DbContext
    {
        public SprintUpDbContext(DbContextOptions<SprintUpDbContext> options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().ToTable("Users");
            modelBuilder.Entity<User>().HasKey(u => u.Id);
        }
    }
}
