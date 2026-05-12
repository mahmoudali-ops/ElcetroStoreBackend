using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using TourSite.Core.Entities;

namespace TourSite.Repository.Data.Contexts
{
    public class ElectroDbContext : IdentityDbContext<User>
    {
        public ElectroDbContext(DbContextOptions<ElectroDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }

        




        // Translation DbSets
  
        public DbSet<Product> Products { get; set; }

        public DbSet<ProductTranslation> ProductTranslations { get; set; }

        public DbSet<Category> Categories { get; set; }

        public DbSet<CategoryTranslation> CategoryTranslations { get; set; }

        public DbSet<ProductImage> ProductImages { get; set; }


        public DbSet<Order> Orders { get; set; }

        public DbSet<OrderItem> OrderItems { get; set; }


        public DbSet<CartItem> CartItems { get; set; }


        public DbSet<Cart> carts { get; set; }

        public DbSet<User> Users { get; set; }


















    }
}
