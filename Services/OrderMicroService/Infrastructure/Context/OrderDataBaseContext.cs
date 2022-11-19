using Microsoft.EntityFrameworkCore;
using OrderMicroService.Model.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OrderMicroService.Infrastructure.Context
{
    public class OrderDataBaseContext : DbContext
    {
        public OrderDataBaseContext(DbContextOptions<OrderDataBaseContext> options)
       : base(options)
        {

        }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderLine> OrderLines { get; set; }
        public DbSet<Product> Products { get; set; }
    }
}
