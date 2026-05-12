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
    public class CategoryConfiguartion : IEntityTypeConfiguration<Category>

    {
        public void Configure(EntityTypeBuilder<Category> builder)
        {

            builder.ToTable("Categories");



            builder.HasMany(c => c.Translations)
           .WithOne(t => t.Category)
           .HasForeignKey(t => t.CategoryId)
           .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(c => c.Products)
            .WithOne(t => t.Category)
            .HasForeignKey(t => t.CategoryId)
            .OnDelete(DeleteBehavior.Cascade);


            builder.Property(u => u.Slug)
           .HasDefaultValue("");

            builder.HasIndex(u => u.Slug)
                   .IsUnique();
        }
    }
}
