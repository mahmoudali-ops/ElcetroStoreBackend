using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TourSite.Core.DTOs.Product;

namespace TourSite.Core.DTOs.Category
{
    public class CategoryDto
    {
        public int Id { get; set; }

        public List<CategoryTranslationDto> Translations { get; set; }

        public string? Slug { get; set; }   // 👈 جديد


        public List<ProductSimpleDto> Products { get; set; }
    }
}
