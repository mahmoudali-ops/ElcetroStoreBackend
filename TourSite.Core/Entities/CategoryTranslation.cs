using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TourSite.Core.Entities
{
    public class CategoryTranslation
    {
        public int Id { get; set; }

        public int CategoryId { get; set; }

        public Category Category { get; set; }

        public string LanguageCode { get; set; } // ar / en

        public string Name { get; set; }
    }
}
