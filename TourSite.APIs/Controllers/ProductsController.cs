using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Webp;
using SixLabors.ImageSharp.Processing;
using System.Text.Json;
using TourSite.APIs.Errors;
using TourSite.Core.DTOs.Product;
using TourSite.Core.DTOs.ProductImage;
using TourSite.Core.Entities;
using TourSite.Repository.Data.Contexts;

namespace TourSite.APIs.Controllers
{
    public class ProductsController : BaseApiController
    {
        private readonly ElectroDbContext _context;
        private readonly IConfiguration _configuration;

     

        public IWebHostEnvironment _env { get; }


        public ProductsController(ElectroDbContext context, IWebHostEnvironment env, IConfiguration configuration)
        {
            _context = context;
            _env = env;
            _configuration = configuration;
        }

        // ================================
        // Client Get All Products
        // ================================
        [HttpGet("client")]
        public async Task<IActionResult> GetAllProducts([FromQuery] string? lang = "en")
        {
            var baseUrl = _configuration["BaseUrl"];

            var products = await _context.Products
                .Include(p => p.Translations)
                .Include(p => p.Images)
                .Include(p => p.Category)
                .Where(p => p.IsAvailable)
                .Select(p => new
                {
                    p.Id,

                    Name = p.Translations
                        .FirstOrDefault(t => t.LanguageCode == lang)!.Name,

                    Description = p.Translations
                        .FirstOrDefault(t => t.LanguageCode == lang)!.Description,

                    p.Price,
                    p.Rate,
                    p.Slug,
                    p.IsAvailable,

                    CategoryId = p.CategoryId,

                    MainImage = baseUrl + p.Images
                        .FirstOrDefault(i => i.IsMain)!.ImageUrl,

                    Images = p.Images.Select(i => new
                    {
                        i.Id,
                        ImageUrl = baseUrl + i.ImageUrl,
                        i.IsMain
                    })
                })
                .ToListAsync();

            return Ok(products);
        }

        // ================================
        // Admin Get All Products
        // ================================
        [HttpGet("admin")]
        public async Task<IActionResult> GetAllProductsAdmin()
        {
            var products = await _context.Products
                .Include(p => p.Translations)
                .Include(p => p.Images)
                .Include(p => p.Category)
                .Select(p => new
                {
                    p.Id,
                    p.Price,
                    p.Rate,
                    p.Slug,
                    p.IsAvailable,
                    p.CategoryId,

                    Translations = p.Translations.Select(t => new
                    {
                        t.Id,
                        t.LanguageCode,
                        t.Name,
                        t.Description
                    }),

                    Images = p.Images.Select(i => new
                    {
                        i.Id,
                        i.ImageUrl,
                        i.IsMain
                    })
                })
                .ToListAsync();

            return Ok(products);
        }

        // ================================
        // Get Product By Slug
        // ================================
        [HttpGet("by-slug/{slug}")]
        public async Task<IActionResult> GetProductBySlug(string slug, [FromQuery] string? lang = "en")
        {
            if (string.IsNullOrWhiteSpace(slug))
                return BadRequest(new APIErrerResponse(400, "Slug is required"));

            var product = await _context.Products
                .Include(p => p.Translations)
                .Include(p => p.Images)
                .Include(p => p.Category)
                .FirstOrDefaultAsync(p => p.Slug == slug);

            if (product == null)
                return NotFound(new APIErrerResponse(404, "Product not found"));

            var translation = product.Translations
                .FirstOrDefault(t => t.LanguageCode == lang);

            var result = new
            {
                product.Id,

                Name = translation?.Name,

                Description = translation?.Description,

                product.Price,
                product.Rate,
                product.Slug,
                product.IsAvailable,
                product.CategoryId,

                Images = product.Images.Select(i => new
                {
                    i.Id,
                    i.ImageUrl,
                    i.IsMain
                })
            };

            return Ok(result);
        }

        // ================================
        // Create Product
        // ================================
        // ======================================
        // Create Product
        // ======================================

        [HttpPost("create")]
        public async Task<IActionResult> CreateProduct([FromForm] CreateProductDto dto)
        {
            // =========================
            // Deserialize Translations
            // =========================
            if (!string.IsNullOrEmpty(dto.TranslationsJson))
            {
                dto.Translations = JsonSerializer.Deserialize<List<ProductTranslationDto>>(
                    dto.TranslationsJson,
                    new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    }
                );
            }

            // =========================
            // Create Product
            // =========================
            var product = new Product
            {
                Price = dto.Price,
                Rate = dto.Rate,
                IsAvailable = dto.IsAvailable,
                CategoryId = dto.CategoryId,
                Slug = dto.Slug,

                Translations = dto.Translations?.Select(t => new ProductTranslation
                {
                    LanguageCode = t.LanguageCode.ToLower(),
                    Name = t.Name,
                    Description = t.Description
                }).ToList()
            };

            // =========================
            // Upload Images
            // =========================
            if (dto.Images != null && dto.Images.Any())
            {
                string uploadDir = Path.Combine(_env.WebRootPath, "images/products");
                Directory.CreateDirectory(uploadDir);

                foreach (var img in dto.Images)
                {
                    if (img.ImageFile == null) continue;

                    string fileName = Guid.NewGuid() + ".webp";
                    string fullPath = Path.Combine(uploadDir, fileName);

                    using (var image = await Image.LoadAsync(img.ImageFile.OpenReadStream()))
                    {
                        image.Mutate(x => x.Resize(1000, 1000));

                        await image.SaveAsync(fullPath, new WebpEncoder
                        {
                            Quality = 80
                        });
                    }

                    product.Images.Add(new ProductImage
                    {
                        ImageUrl = $"images/products/{fileName}",
                        IsMain = img.IsMain
                    });
                }
            }

            await _context.Products.AddAsync(product);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Product created successfully" });
        }
        // ================================
        // Update Product
        // ================================
        [HttpPut("update/{id}")]
        public async Task<IActionResult> UpdateProduct(int id, [FromForm] CreateProductDto dto)
        {
            var product = await _context.Products
                .Include(p => p.Images)
                .Include(p => p.Translations)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (product == null)
                return NotFound();

            // =========================
            // JSON Translations
            // =========================
            if (!string.IsNullOrEmpty(dto.TranslationsJson))
            {
                dto.Translations = JsonSerializer.Deserialize<List<ProductTranslationDto>>(
                    dto.TranslationsJson,
                    new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    }
                );
            }

            // =========================
            // Update main fields
            // =========================
            product.Price = dto.Price;
            product.Rate = dto.Rate;
            product.IsAvailable = dto.IsAvailable;
            product.CategoryId = dto.CategoryId;
            product.Slug = dto.Slug;

            // =========================
            // Update translations
            // =========================
            product.Translations.Clear();

            if (dto.Translations != null)
            {
                foreach (var t in dto.Translations)
                {
                    product.Translations.Add(new ProductTranslation
                    {
                        LanguageCode = t.LanguageCode.ToLower(),
                        Name = t.Name,
                        Description = t.Description
                    });
                }
            }

            // =========================
            // Replace Images
            // =========================
            if (dto.Images != null && dto.Images.Any())
            {
                string uploadDir = Path.Combine(_env.WebRootPath, "images/products");
                Directory.CreateDirectory(uploadDir);

                // delete old images (files + db)
                foreach (var old in product.Images)
                {
                    var oldPath = Path.Combine(_env.WebRootPath, old.ImageUrl);

                    if (System.IO.File.Exists(oldPath))
                        System.IO.File.Delete(oldPath);
                }

                product.Images.Clear();

                foreach (var img in dto.Images)
                {
                    if (img.ImageFile == null) continue;

                    string fileName = Guid.NewGuid() + ".webp";
                    string fullPath = Path.Combine(uploadDir, fileName);

                    using (var image = await Image.LoadAsync(img.ImageFile.OpenReadStream()))
                    {
                        image.Mutate(x => x.Resize(1000, 1000));

                        await image.SaveAsync(fullPath, new WebpEncoder
                        {
                            Quality = 80
                        });
                    }

                    product.Images.Add(new ProductImage
                    {
                        ImageUrl = $"images/products/{fileName}",
                        IsMain = img.IsMain
                    });
                }
            }

            await _context.SaveChangesAsync();

            return Ok(new { message = "Product updated successfully" });
        }

        // ================================
        // Delete Product
        // ================================
        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            if (id <= 0)
                return BadRequest(new APIErrerResponse(400, "Invalid Id"));

            var product = await _context.Products
                .FirstOrDefaultAsync(p => p.Id == id);

            if (product == null)
                return NotFound(new APIErrerResponse(404, $"No Product Found With Id : {id}"));

            _context.Products.Remove(product);

            await _context.SaveChangesAsync();

            return Ok(new
            {
                message = "Product deleted successfully"
            });
        }

        // ================================
        // Get Product For Update
        // ================================
        [HttpGet("get-product-for-update/{id}")]
        public async Task<IActionResult> GetProductForUpdate(int id)
        {
            var product = await _context.Products
                .Include(p => p.Translations)
                .Include(p => p.Images)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (product == null)
                return NotFound(new APIErrerResponse(404, "Product not found"));

            var dto = new UpdateProductDto
            {
                Id = product.Id,
                Price = product.Price,
                Rate = product.Rate,
                IsAvailable = product.IsAvailable,
                CategoryId = product.CategoryId,
                Slug = product.Slug,

                Translations = product.Translations.Select(t => new ProductTranslationDto
                {
                    Id = t.Id,
                    ProductId = t.ProductId,
                    LanguageCode = t.LanguageCode,
                    Name = t.Name,
                    Description = t.Description
                }).ToList(),

                ImageUrls = product.Images.Select(i => new ProductImageDto
                {
                    Id = i.Id,
                    ProductId = i.ProductId,
                    ImageUrl = i.ImageUrl,
                    IsMain = i.IsMain
                }).ToList()
            };

            return Ok(dto);
        }
    }
}
