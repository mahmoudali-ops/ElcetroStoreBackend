using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TourSite.Core.DTOs.Order;
using TourSite.Core.Entities;
using TourSite.Repository.Data.Contexts;

namespace TourSite.APIs.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly ElectroDbContext _context;

        public OrderController(ElectroDbContext context)
        {
            _context = context;
        }

        // ==================================================
        // CREATE ORDER
        // ==================================================
        [HttpPost("create")]
        public async Task<IActionResult> CreateOrder([FromBody] OrderCreateDto dto)
        {
            if (dto.OrderItems == null || !dto.OrderItems.Any())
                return BadRequest("Order must contain items");

            var order = new Order
            {
                UserId = dto.UserId,
                OrderDate = DateTime.UtcNow,
                Status = OrderStatus.Pending,
                PaymentMethod = dto.PaymentMethod,
                OrderItems = new List<OrderItem>()
            };

            decimal total = 0;

            foreach (var item in dto.OrderItems)
            {
                var product = await _context.Products
                    .Include(p => p.Translations)
                    .FirstOrDefaultAsync(p => p.Id == item.ProductId);

                if (product == null)
                    return BadRequest($"Product {item.ProductId} not found");

                order.OrderItems.Add(new OrderItem
                {
                    ProductId = item.ProductId,
                    Quantity = item.Quantity,
                    Price = product.Price
                });

                total += product.Price * item.Quantity;
            }

            order.TotalPrice = total;

            await _context.Orders.AddAsync(order);
            await _context.SaveChangesAsync();

            return Ok(new
            {
                message = "Order created successfully",
                orderId = order.Id,
                totalPrice = order.TotalPrice
            });
        }

        // ==================================================
        // GET ALL ORDERS (ADMIN DASHBOARD)
        // ==================================================
        [HttpGet("admin")]
        public async Task<IActionResult> GetAllOrders()
        {
            var orders = await _context.Orders
                .Include(o => o.OrderItems)
                    .ThenInclude(i => i.Product)
                        .ThenInclude(p => p.Translations)
                .ToListAsync();

            var result = orders.Select(order => new OrderDto
            {
                Id = order.Id,
                UserId = order.UserId,
                OrderDate = order.OrderDate,
                TotalPrice = order.TotalPrice,

                Status = order.Status.ToString(),
                PaymentMethod = order.PaymentMethod.ToString(),

                OrderItems = order.OrderItems.Select(i => new OrderItemDto
                {
                    Id = i.Id,
                    ProductId = i.ProductId,

                    ProductName = i.Product.Translations
                        .FirstOrDefault(t => t.LanguageCode == "en")!.Name,

                    Quantity = i.Quantity,
                    Price = i.Price
                }).ToList()
            });

            return Ok(result);
        }

        // ==================================================
        // GET ORDER BY ID
        // ==================================================
        [HttpGet("{id}")]
        public async Task<IActionResult> GetOrderById(int id)
        {
            var order = await _context.Orders
                .Include(o => o.OrderItems)
                    .ThenInclude(i => i.Product)
                        .ThenInclude(p => p.Translations)
                .FirstOrDefaultAsync(o => o.Id == id);

            if (order == null)
                return NotFound();

            var result = new OrderDto
            {
                Id = order.Id,
                UserId = order.UserId,
                OrderDate = order.OrderDate,
                TotalPrice = order.TotalPrice,

                Status = order.Status.ToString(),
                PaymentMethod = order.PaymentMethod.ToString(),

                OrderItems = order.OrderItems.Select(i => new OrderItemDto
                {
                    Id = i.Id,
                    ProductId = i.ProductId,

                    ProductName = i.Product.Translations
                        .FirstOrDefault(t => t.LanguageCode == "en")!.Name,

                    Quantity = i.Quantity,
                    Price = i.Price
                }).ToList()
            };

            return Ok(result);
        }

        // ==================================================
        // GET USER ORDERS
        // ==================================================
        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetUserOrders(string userId)
        {
            var orders = await _context.Orders
                .Include(o => o.OrderItems)
                    .ThenInclude(i => i.Product)
                        .ThenInclude(p => p.Translations)
                .Where(o => o.UserId == userId)
                .ToListAsync();

            var result = orders.Select(order => new OrderDto
            {
                Id = order.Id,
                UserId = order.UserId,
                OrderDate = order.OrderDate,
                TotalPrice = order.TotalPrice,

                Status = order.Status.ToString(),
                PaymentMethod = order.PaymentMethod.ToString(),

                OrderItems = order.OrderItems.Select(i => new OrderItemDto
                {
                    Id = i.Id,
                    ProductId = i.ProductId,

                    ProductName = i.Product.Translations
                        .FirstOrDefault(t => t.LanguageCode == "en")!.Name,

                    Quantity = i.Quantity,
                    Price = i.Price
                }).ToList()
            });

            return Ok(result);
        }

        // ==================================================
        // UPDATE ORDER STATUS
        // ==================================================
        [HttpPut("update-status/{id}")]
        public async Task<IActionResult> UpdateStatus(int id, [FromQuery] OrderStatus status)
        {
            var order = await _context.Orders.FindAsync(id);

            if (order == null)
                return NotFound();

            order.Status = status;

            await _context.SaveChangesAsync();

            return Ok(new { message = "Status updated successfully" });
        }

        // ==================================================
        // DELETE ORDER
        // ==================================================
        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> DeleteOrder(int id)
        {
            var order = await _context.Orders
                .Include(o => o.OrderItems)
                .FirstOrDefaultAsync(o => o.Id == id);

            if (order == null)
                return NotFound();

            _context.OrderItems.RemoveRange(order.OrderItems);
            _context.Orders.Remove(order);

            await _context.SaveChangesAsync();

            return Ok(new { message = "Order deleted successfully" });
        }
    }
}
