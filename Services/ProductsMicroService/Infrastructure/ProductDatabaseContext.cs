using Microsoft.EntityFrameworkCore;
using ProductsMicroService.Domain;
using System.Collections.Generic;

namespace ProductsMicroService.Infrastructure
{
    public class ProductDatabaseContext : DbContext
    {
        public ProductDatabaseContext(DbContextOptions<ProductDatabaseContext> options) : base(options)
        {

        }
        public DbSet<Product> Products { get; set; }
        public DbSet<Category> Categories { get; set; }
    }
}
