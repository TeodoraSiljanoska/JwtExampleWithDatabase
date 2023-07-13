using JwtExampleWithDatabase.Models;
using Microsoft.EntityFrameworkCore;

namespace JwtExampleWithDatabase.Data
{
    public class DataContext : DbContext
    {

        public DataContext(DbContextOptions options) : base(options)
        {

        }

        public DbSet<User> Users { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>(entity =>
            {
                entity.Property(u => u.Id)
                    .ValueGeneratedOnAdd();

                entity.Property(u => u.Username)
                    .IsRequired();

                entity.Property(u => u.Password)
                    .IsRequired();

                entity.Property(u => u.Role)
                    .IsRequired();
            });

            base.OnModelCreating(modelBuilder);
        }



        }
    }



