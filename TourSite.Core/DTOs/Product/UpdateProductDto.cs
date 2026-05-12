using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TourSite.Core.DTOs.ProductImage;

namespace TourSite.Core.DTOs.Product
{
    public class UpdateProductDto
    {
        public int Id { get; set; }

        public decimal Price { get; set; }

        public decimal Rate { get; set; }

        public bool IsAvailable { get; set; }

        public int CategoryId { get; set; }

        public string? Slug { get; set; }   // 👈 جديد


        public List<ProductTranslationDto> Translations { get; set; }

        public List<ProductImageDto> ImageUrls { get; set; }

        public string? TranslationsJson { get; set; }

    }
}
