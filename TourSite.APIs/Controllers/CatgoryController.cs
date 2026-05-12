using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;
using TourSite.Core.DTOs.Category;
using TourSite.Core.DTOs.Product;
using TourSite.Core.Entities;
using TourSite.Repository.Data.Contexts;

namespace TourSite.APIs.Controllers
{
   
    public class CatgoryController : BaseApiController
    {
        private readonly ElectroDbContext _context;

        public CatgoryController(ElectroDbContext context)
        {
            _context = context;
        }

        [HttpPost("create")]
        public async Task<IActionResult> CreateCategory([FromForm] CreateCategoryDto dto)
        {
            // =========================
            // Deserialize JSON
            // =========================
            if (!string.IsNullOrEmpty(dto.TranslationsJson))
            {
                dto.Translations = JsonSerializer.Deserialize<List<CategoryTranslationDto>>(
                    dto.TranslationsJson,
                    new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    }
                );
            }

            // =========================
            // Create Entity
            // =========================
            var category = new Category
            {
                Slug = dto.Slug,

                Translations = dto.Translations?.Select(t => new CategoryTranslation
                {
                    LanguageCode = t.LanguageCode.ToLower(),
                    Name = t.Name
                }).ToList()
            };

            await _context.Categories.AddAsync(category);
            await _context.SaveChangesAsync();

            return Ok(new
            {
                message = "Category created successfully"
            });
        }

        [HttpPut("update/{id}")]
        public async Task<IActionResult> UpdateCategory(int id, [FromForm] UpdateCategoryDto dto)
        {
            var category = await _context.Categories
                .Include(c => c.Translations)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (category == null)
                return NotFound();

            // =========================
            // JSON Deserialize
            // =========================
            if (!string.IsNullOrEmpty(dto.TranslationsJson))
            {
                dto.Translations = JsonSerializer.Deserialize<List<CategoryTranslationDto>>(
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
            category.Slug = dto.Slug;

            // =========================
            // Update translations (replace strategy)
            // =========================
            category.Translations.Clear();

            if (dto.Translations != null)
            {
                foreach (var t in dto.Translations)
                {
                    category.Translations.Add(new CategoryTranslation
                    {
                        LanguageCode = t.LanguageCode.ToLower(),
                        Name = t.Name
                    });
                }
            }

            await _context.SaveChangesAsync();

            return Ok(new
            {
                message = "Category updated successfully"
            });
        }


        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            var category = await _context.Categories
                .FirstOrDefaultAsync(c => c.Id == id);

            if (category == null)
                return NotFound();

            _context.Categories.Remove(category);
            await _context.SaveChangesAsync();

            return Ok(new
            {
                message = "Category deleted successfully"
            });
        }

        [HttpGet("client")]
        public async Task<IActionResult> GetAllCategories([FromQuery] string lang = "en")
        {
            var categories = await _context.Categories
                .Include(c => c.Translations)
                .Include(c => c.Products)
                .ThenInclude(p => p.Translations)
                .Select(c => new CategoryDto
                {
                    Id = c.Id,
                    Slug = c.Slug,

                    Translations = c.Translations.Select(t => new CategoryTranslationDto
                    {
                        LanguageCode = t.LanguageCode,
                        Name = t.Name
                    }).ToList(),

                    Products = c.Products.Select(p => new ProductSimpleDto
                    {
                        Id = p.Id,
                        Price = p.Price,
                        Slug = p.Slug,

                        Name = p.Translations
                            .FirstOrDefault(t => t.LanguageCode == lang)!.Name
                    }).ToList()
                })
                .ToListAsync();

            return Ok(categories);
        }


        [HttpGet("{id}")]
        public async Task<IActionResult> GetCategoryById(int id)
        {
            var category = await _context.Categories
                .Include(c => c.Translations)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (category == null)
                return NotFound();

            var dto = new CategoryDto
            {
                Id = category.Id,
                Slug = category.Slug,

                Translations = category.Translations.Select(t => new CategoryTranslationDto
                {
                    LanguageCode = t.LanguageCode,
                    Name = t.Name
                }).ToList()
            };

            return Ok(dto);
        }
    }
}
