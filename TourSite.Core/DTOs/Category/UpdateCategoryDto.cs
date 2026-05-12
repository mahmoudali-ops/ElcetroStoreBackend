using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TourSite.Core.DTOs.Category
{
    public class UpdateCategoryDto
    {
        public int Id { get; set; }

        public List<CategoryTranslationDto> Translations { get; set; }
        public string? Slug { get; set; }   // 👈 جديد
        public string? TranslationsJson { get; set; }


    }
}
