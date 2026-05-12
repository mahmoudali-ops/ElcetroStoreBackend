using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TourSite.Core.DTOs.User;
using TourSite.Core.Entities;
using TourSite.Repository.Data.Contexts;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace TourSite.APIs.Controllers
{

    public class CustomerController : BaseApiController
    {

        private readonly ElectroDbContext _context;

        public CustomerController(ElectroDbContext context)
        {
            _context = context;
        }

        // ==================================================
        // GET CUSTOMER ORDERS (TRACKING)
        // ==================================================
        [HttpGet("orders/{userId}")]
        public async Task<IActionResult> GetCustomerOrders(string userId)
        {
            var orders = await _context.Orders
                .Include(o => o.OrderItems)
                    .ThenInclude(i => i.Product)
                        .ThenInclude(p => p.Translations)
                .Where(o => o.UserId == userId)
                .OrderByDescending(o => o.OrderDate)
                .ToListAsync();

            var result = orders.Select(order => new
            {
                order.Id,
                order.OrderDate,
                order.TotalPrice,

                // 👇 أهم حاجة للعميل
                Status = order.Status.ToString(),

                StatusStep = GetStatusStep(order.Status),

                PaymentMethod = order.PaymentMethod.ToString(),

                Items = order.OrderItems.Select(i => new
                {
                    i.ProductId,

                    ProductName = i.Product.Translations
                        .FirstOrDefault(t => t.LanguageCode == "en")!.Name,

                    i.Quantity,
                    i.Price,
                    Total = i.Price * i.Quantity
                })
            });

            return Ok(result);
        }

        // ==================================================
        // GET SINGLE ORDER DETAILS (CUSTOMER)
        // ==================================================
        [HttpGet("order/{userId}/{orderId}")]
        public async Task<IActionResult> GetOrderDetails(string userId, int orderId)
        {
            var order = await _context.Orders
                .Include(o => o.OrderItems)
                    .ThenInclude(i => i.Product)
                        .ThenInclude(p => p.Translations)
                .FirstOrDefaultAsync(o => o.Id == orderId && o.UserId == userId);

            if (order == null)
                return NotFound();

            var result = new
            {
                order.Id,
                order.OrderDate,
                order.TotalPrice,

                Status = order.Status.ToString(),
                StatusStep = GetStatusStep(order.Status),

                PaymentMethod = order.PaymentMethod.ToString(),

                Items = order.OrderItems.Select(i => new
                {
                    i.ProductId,

                    ProductName = i.Product.Translations
                        .FirstOrDefault(t => t.LanguageCode == "en")!.Name,

                    i.Quantity,
                    i.Price,
                    Total = i.Price * i.Quantity
                })
            };

            return Ok(result);
        }

        // ==================================================
        // ORDER STATUS FLOW (Helper)
        // ==================================================
        private string GetStatusStep(OrderStatus status)
        {
            return status switch
            {
                OrderStatus.Pending => "Order Placed",
                OrderStatus.Confirmed => "Order Confirmed",
                OrderStatus.Preparing => "Preparing Order",
                OrderStatus.OutForDelivery => "Out for Delivery",
                OrderStatus.Delivered => "Delivered",
                OrderStatus.Cancelled => "Cancelled",
                _ => "Unknown"
            };
        }


        [HttpPost("checkout")]
        public async Task<IActionResult> Checkout([FromBody] CheckoutDto dto)
        {
            var cart = await _context.carts
                .Include(c => c.CartItems)
                    .ThenInclude(i => i.Product)
                .FirstOrDefaultAsync(c => c.UserId == dto.UserId);

            if (cart == null || !cart.CartItems.Any())
                return BadRequest("Cart is empty");

            decimal total = cart.CartItems.Sum(i => i.Product.Price * i.Quantity);

            // =========================
            // CASH ON DELIVERY
            // =========================
            if (dto.PaymentMethod == PaymentMethod.CashOnDelivery)
            {
                var order = new Order
                {
                    UserId = dto.UserId,
                    OrderDate = DateTime.UtcNow,
                    Status = OrderStatus.Pending,
                    PaymentMethod = PaymentMethod.CashOnDelivery,
                    TotalPrice = total,
                    OrderItems = cart.CartItems.Select(i => new OrderItem
                    {
                        ProductId = i.ProductId,
                        Quantity = i.Quantity,
                        Price = i.Product.Price
                    }).ToList()
                };

                _context.Orders.Add(order);

                // clear cart
                _context.CartItems.RemoveRange(cart.CartItems);

                await _context.SaveChangesAsync();

                return Ok(new
                {
                    message = "Order placed successfully (Cash)",
                    orderId = order.Id,
                    totalPrice = total
                });
            }

            // =========================
            // ONLINE PAYMENT (Stripe)
            // =========================
            if (dto.PaymentMethod == PaymentMethod.Online)
            {
                var options = new Stripe.Checkout.SessionCreateOptions
                {
                    PaymentMethodTypes = new List<string> { "card" },
                    LineItems = cart.CartItems.Select(i => new Stripe.Checkout.SessionLineItemOptions
                    {
                        Quantity = i.Quantity,
                        PriceData = new Stripe.Checkout.SessionLineItemPriceDataOptions
                        {
                            Currency = "usd",
                            UnitAmount = (long)(i.Product.Price * 100), // cents
                            ProductData = new Stripe.Checkout.SessionLineItemPriceDataProductDataOptions
                            {
                                Name = i.Product.Translations.FirstOrDefault(t => t.LanguageCode == "en")!.Name
                            }
                        }
                    }).ToList(),

                    Mode = "payment",
                    SuccessUrl = "https://yourfrontend.com/success",
                    CancelUrl = "https://yourfrontend.com/cancel"
                };

                var service = new Stripe.Checkout.SessionService();
                var session = await service.CreateAsync(options);

                return Ok(new
                {
                    url = session.Url,
                    totalPrice = total
                });
            }

            return BadRequest("Invalid payment method");
        }
    }
}
