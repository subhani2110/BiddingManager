using DataLayer.Entities;
using Microsoft.EntityFrameworkCore;

namespace DataLayer
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        public DbSet<Asset> Assets { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Bid> Bids { get; set; }
        public DbSet<Review> Reviews { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().HasData(
                new User
                {
                    Id = 1,
                    Username = "admin",
                    Name = "Admin",
                    Email = "admin@mail.com",
                    Password = "123Password1$",
                    Role = "Admin"
                },
                new User
                {
                    Id = 2,
                    Username = "buyer",
                    Name = "Buyer",
                    CanBuy = true,
                    Email = "buyer@mail.com",
                    Password = "123Password1$",
                    Role = "Client"
                },
                new User
                {
                    Id = 3,
                    Username = "seller",
                    Name = "Seller",
                    CanSell = true,
                    Email = "seller@mail.com",
                    Password = "123Password1$",
                    Role = "Client"
                }
            );
        }
    }

}
