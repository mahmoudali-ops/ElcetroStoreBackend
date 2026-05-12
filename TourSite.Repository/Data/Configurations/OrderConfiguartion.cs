using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TourSite.Core.Entities;

namespace TourSite.Repository.Data.Configurations
{
    public class OrdertConfiguartion : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            builder.ToTable("Orders");


            builder.HasMany(c => c.OrderItems)
            .WithOne(t => t.Order)
            .HasForeignKey(t => t.OrderId)
            .OnDelete(DeleteBehavior.Cascade);


        }
    }
    public class OrderItemsConfiguartion : IEntityTypeConfiguration<OrderItem>
    {
        public void Configure(EntityTypeBuilder<OrderItem> builder)
        {
            builder.ToTable("OrderItems");



        }
    }


    public class CartConfiguartion : IEntityTypeConfiguration<Cart>
    {
        public void Configure(EntityTypeBuilder<Cart> builder)
        {
            builder.ToTable("Carts");

            builder.HasMany(c => c.CartItems)
             .WithOne(t => t.Cart)
             .HasForeignKey(t => t.CartId)
             .OnDelete(DeleteBehavior.Cascade);

        }
    }

 

}
