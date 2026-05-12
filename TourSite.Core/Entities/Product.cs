using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TourSite.Core.Entities
{
    public class Product
    {
        public int Id { get; set; }

        public decimal Price { get; set; }

        public decimal Rate { get; set; }

        public string? Slug { get; set; }   // 👈 جديد


        public bool IsAvailable { get; set; }

        public int CategoryId { get; set; }

        public Category Category { get; set; }

        public ICollection<ProductTranslation> Translations { get; set; }

        public ICollection<ProductImage> Images { get; set; }

        public ICollection<OrderItem> OrderItems { get; set; }

        public ICollection<CartItem> CartItems { get; set; }
    }
}
