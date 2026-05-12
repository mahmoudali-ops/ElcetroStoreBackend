using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TourSite.Core.DTOs.Product
{
    public class ProductTranslationDto
    {
        public int Id { get; set; }

        public int ProductId { get; set; }


        public string LanguageCode { get; set; } // ar / en

        public string Name { get; set; }

        public string Description { get; set; }
    }
}
