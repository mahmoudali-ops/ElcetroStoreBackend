using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TourSite.Core.DTOs.ProductImage;

namespace TourSite.Core.DTOs.Product
{
    public class CreateProductDto
    {
        public decimal Price { get; set; }

        public decimal Rate { get; set; }

        public bool IsAvailable { get; set; }

        public int CategoryId { get; set; }

        public string? Slug { get; set; }

        // JSON للترجمات
        public string? TranslationsJson { get; set; }

        public List<ProductTranslationDto>? Translations { get; set; }

        // Upload images
        public List<ProductCreateImageDto>? Images { get; set; }

    }
}
