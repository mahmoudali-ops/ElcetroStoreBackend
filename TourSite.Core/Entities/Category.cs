using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TourSite.Core.Entities
{
    public class Category
    {
        public int Id { get; set; }

        public ICollection<CategoryTranslation> Translations { get; set; }

        public string? Slug { get; set; }   // 👈 جديد


        public ICollection<Product> Products { get; set; }
    }
}
