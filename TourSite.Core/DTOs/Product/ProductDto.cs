using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TourSite.Core.DTOs.ProductImage;

namespace TourSite.Core.DTOs.Product
{
    public class ProductDto
    {
        public int Id { get; set; }

        public decimal Price { get; set; }

        public decimal Rate { get; set; }

        public bool IsAvailable { get; set; }

        public string? Slug { get; set; }

        public int CategoryId { get; set; }

        // 👇 خليها Display فقط
        public string CategoryName { get; set; }

        // 👇 أهم حاجة للـ multilingual
        public string ProductName { get; set; }

        public List<ProductTranslationDto> Translations { get; set; }

        public List<ProductImageDto> Images { get; set; }
    }
}
