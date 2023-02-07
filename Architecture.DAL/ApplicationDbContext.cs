using Architecture.DAL.Configurations;
using Architecture.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Architecture.DAL
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
            //Database.EnsureCreated();
        }

        public DbSet<Member> Members { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<File> Files { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new MemberConfiguration());
            modelBuilder.ApplyConfiguration(new UserConfiguration());
            modelBuilder.ApplyConfiguration(new FileConfiguration());
        }
    }
}
