using ECommerceBE.Models;
using Microsoft.EntityFrameworkCore;

namespace ECommerceBE.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }

        public DbSet<Product> Products { get; set; }
        public DbSet<Supplier> Suppliers { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<ProductsCategories> ProductsCategories { get; set; }
    }
}
