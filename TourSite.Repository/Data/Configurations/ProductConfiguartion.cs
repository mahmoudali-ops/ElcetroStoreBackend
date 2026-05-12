using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.Xml;
using System.Text;
using System.Threading.Tasks;
using TourSite.Core.Entities;

namespace TourSite.Repository.Data.Configurations
{
    public class ProductConfiguartion : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.ToTable("Products");

            builder.Property(t => t.IsAvailable)
                .HasDefaultValue(true);

   

            builder.HasMany(c => c.Translations)
           .WithOne(t => t.Product)
           .HasForeignKey(t => t.ProductId)
           .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(c => c.Images)
            .WithOne(t => t.Product)
            .HasForeignKey(t => t.ProductId)
            .OnDelete(DeleteBehavior.Cascade);


            builder.Property(u => u.Slug)
           .HasDefaultValue(""); // القيمة الافتراضية

            builder.HasIndex(u => u.Slug)
                   .IsUnique(); // index فريد


        }

    }
}
