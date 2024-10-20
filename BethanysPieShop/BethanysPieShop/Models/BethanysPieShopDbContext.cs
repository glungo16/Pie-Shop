using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;

namespace BethanysPieShop.Models
{
    public class BethanysPieShopDbContext : IdentityDbContext
    {
        public BethanysPieShopDbContext(DbContextOptions<BethanysPieShopDbContext> options) : base(options)
        {  
        }

        public DbSet<Category> Categories { get; set; }
        public DbSet<Pie> Pies { get; set; }
        public DbSet<ShoppingCartItem> ShoppingCartItems { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderDetail> OrderDetails { get; set; }

        // package manager:
        // initial migration: add-migration <name> (add-migration InitialMigration)
        // update-database: brings database up to date

        // later on when we add new stuff:
        // add-migration <name> (add-migration OrderAdded)
        // update-database: brings database up to date

    }
}
