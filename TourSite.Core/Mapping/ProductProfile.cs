using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TourSite.Core.DTOs.Product;
using TourSite.Core.Entities;

namespace TourSite.Core.Mapping
{
    public class ProductProfile:Profile
    {
        public ProductProfile()
        {
            CreateMap<Product, ProductDto>().
                ForMember(
                    dest => dest.Translations,
                    opt => opt.MapFrom(
                        src => src.Translations
                    )
                );

            CreateMap<ProductTranslation, ProductTranslationDto>();
            CreateMap<CreateProductDto, Product>();



        }
    }
}
