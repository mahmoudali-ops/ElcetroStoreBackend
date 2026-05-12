using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TourSite.Core.DTOs.Product
{
    public class ProductSimpleDto
    {
        public int Id { get; set; }

        public decimal Price { get; set; }

        public decimal Rate { get; set; }

        public string Name { get; set; }
        public string? Slug { get; set; }   // 👈 جديد


        public string ImageUrl { get; set; }
    }
}
