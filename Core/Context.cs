using Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace Core
{
    public class Context : DbContext
    {
        private readonly ConnectionSetting _setting;

        public DbSet<User> Users { get; set; }

        public Context(ConnectionSetting setting)
        {
            _setting = setting;
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<User>()
                .Property(u => u.Id)
                .ValueGeneratedOnAdd();

            builder.Entity<User>()
                .HasIndex(u => u.Phone)
                .IsUnique();

            builder.Entity<User>()
                .HasIndex(u => u.Email)
                .IsUnique();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {

            optionsBuilder.UseSqlServer(_setting.ConnectionString, b => b.MigrationsAssembly("UserTask"));
        }
    }
}
