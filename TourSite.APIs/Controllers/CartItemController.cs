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
  
    public class CartItemController : BaseApiController
    {
        private readonly ElectroDbContext _context;

        public CartItemController(ElectroDbContext context)
        {
            _context = context;
        }

    }
}
