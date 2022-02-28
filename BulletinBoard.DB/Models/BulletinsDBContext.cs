using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace BulletinBoard.DB.Models
{
    public class BulletinsDBContext : DbContext
    {
        public DbSet<Bulletin> Bulletins { get; set; }

        public BulletinsDBContext(DbContextOptions<BulletinsDBContext> options)
            : base(options)
        {
            Database.EnsureCreated();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

            modelBuilder.Entity<Bulletin>(entity =>
            {
                entity.HasKey(k => k.ID).HasName("ID");
            });
        }
    }
}
