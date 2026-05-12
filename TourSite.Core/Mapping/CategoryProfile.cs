using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TourSite.Core.DTOs.Category;
using TourSite.Core.DTOs.Product;
using TourSite.Core.Entities;

namespace TourSite.Core.Mapping
{
    public class CategoryProfile : Profile
    {
        public CategoryProfile()
        {
            CreateMap<Category, CategoryDto>().
                ForMember(
                    dest => dest.Translations,
                    opt => opt.MapFrom(
                        src => src.Translations
                    )
                );

            CreateMap<CategoryTranslation, CategoryTranslationDto>();

        }
    }
}
