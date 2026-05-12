using Store.Core.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TourSite.Core.DTOs.Product;
using TourSite.Core.DTOs.ProductImage;
using TourSite.Core.Specification.products;

namespace TourSite.Core.Servicies.Contract
{
    public interface IProductService
    {
        Task<PageinationResponse<ProductDto>> GetAllProductsAsync(ProdcutsSpecParams transSpecParams);
        Task<PageinationResponse<ProductDto>> GetAllProductsAdminAsync(ProdcutsSpecParams transSpecParams);

        Task<ProductDto> GetProductsByIdAsync(string slug, string? lang = "en");

        Task AddProductsAsync(CreateProductDto TransferDto);
        Task<Boolean> UpdateProduct(CreateProductDto dto, int id);
        Task<Boolean> DeleteProduct(int id);
    }
}
