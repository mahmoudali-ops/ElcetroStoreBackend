using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TourSite.Core.DTOs.ProductImage
{
    public class ProductCreateImageDto
    {
        public IFormFile? ImageFile { get; set; }

        public bool IsMain { get; set; }
    }
}
