using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TourSite.Core.Entities
{
    public class ProductTranslation
    {
        public int Id { get; set; }

        public int ProductId { get; set; }

        public Product Product { get; set; }

        public string LanguageCode { get; set; } // ar / en

        public string Name { get; set; }

        public string Description { get; set; }
    }
}
