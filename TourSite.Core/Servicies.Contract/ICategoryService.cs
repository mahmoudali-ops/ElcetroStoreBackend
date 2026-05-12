using Store.Core.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TourSite.Core.DTOs.Category;
using TourSite.Core.DTOs.Product;
using TourSite.Core.Specification.category;
using TourSite.Core.Specification.products;

namespace TourSite.Core.Servicies.Contract
{
    public interface ICategoryService
    {
        Task<PageinationResponse<CategoryDto>> GetAllCategoriesAsync(CategorypecParams transSpecParams);
        Task<ProductDto> GetCategoryByIdAsync(string slug, string? lang = "en");

        Task AddCategoryAsync(CreateCategoryDto TransferDto);
        Task<Boolean> UpdateCategory(CreateCategoryDto dto, int id);
        Task<Boolean> DeleteCategory(int id);
    }
}
