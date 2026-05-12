using AutoMapper;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TourSite.Core.DTOs.ProductImage;
using TourSite.Core.Entities;

namespace TourSite.Core.Mapping
{
    public class ProductImageProfile:Profile
    {
        
            public ProductImageProfile(IConfiguration configuration)
            {
            CreateMap<ProductImage, ProductImageDto>()
                                .ForMember(d => d.ImageUrl, options => options.MapFrom(s => $"{configuration["BaseUrl"]}{s.ImageUrl}"));

            CreateMap<ProductCreateImageDto, ProductImage>()
           .ForMember(dest => dest.ImageUrl, opt => opt.Ignore());// لأننا هنرفع الصورة يدوي


            }
        

    }
}
