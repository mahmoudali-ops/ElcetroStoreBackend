using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TourSite.Core.DTOs.Cart;
using TourSite.Core.Entities;
using TourSite.Repository.Data.Contexts;

namespace TourSite.APIs.Controllers
{
  
    public class CartController : BaseApiController
    {
        private readonly ElectroDbContext _context;

        public CartController(ElectroDbContext context)
        {
            _context = context;
        }

        // ==================================================
        // GET OR CREATE CART (Helper)
        // ==================================================
        private async Task<Cart> GetOrCreateCart(string userId)
        {
            var cart = await _context.carts
                .Include(c => c.CartItems)
                .FirstOrDefaultAsync(c => c.UserId == userId);

            if (cart == null)
            {
                cart = new Cart
                {
                    UserId = userId,
                    CartItems = new List<CartItem>()
                };

                _context.carts.Add(cart);
                await _context.SaveChangesAsync();
            }

            return cart;
        }

        // ==================================================
        // ADD ITEM TO CART
        // ==================================================
        [HttpPost("add")]
        public async Task<IActionResult> AddToCart([FromBody] CartItemDto dto)
        {
            var cart = await GetOrCreateCart(dto.CartId.ToString()); // هنبدله بالUserId تحت

            var existingItem = cart.CartItems
                .FirstOrDefault(i => i.ProductId == dto.ProductId);

            if (existingItem != null)
            {
                existingItem.Quantity += dto.Quantity;
            }
            else
            {
                cart.CartItems.Add(new CartItem
                {
                    ProductId = dto.ProductId,
                    Quantity = dto.Quantity
                });
            }

            await _context.SaveChangesAsync();

            return Ok(new { message = "Item added to cart" });
        }

        // ==================================================
        // INCREASE QUANTITY
        // ==================================================
        [HttpPut("increase/{cartItemId}")]
        public async Task<IActionResult> Increase(int cartItemId)
        {
            var item = await _context.CartItems.FindAsync(cartItemId);

            if (item == null)
                return NotFound();

            item.Quantity++;

            await _context.SaveChangesAsync();

            return Ok();
        }

        // ==================================================
        // DECREASE QUANTITY
        // ==================================================
        [HttpPut("decrease/{cartItemId}")]
        public async Task<IActionResult> Decrease(int cartItemId)
        {
            var item = await _context.CartItems.FindAsync(cartItemId);

            if (item == null)
                return NotFound();

            item.Quantity--;

            if (item.Quantity <= 0)
            {
                _context.CartItems.Remove(item);
            }

            await _context.SaveChangesAsync();

            return Ok();
        }

        // ==================================================
        // REMOVE ITEM
        // ==================================================
        [HttpDelete("remove/{cartItemId}")]
        public async Task<IActionResult> RemoveItem(int cartItemId)
        {
            var item = await _context.CartItems.FindAsync(cartItemId);

            if (item == null)
                return NotFound();

            _context.CartItems.Remove(item);

            await _context.SaveChangesAsync();

            return Ok(new { message = "Item removed" });
        }

        // ==================================================
        // GET CART (CHECKOUT VIEW)
        // ==================================================
        [HttpGet("checkout/{userId}")]
        public async Task<IActionResult> GetCheckout(string userId)
        {
            var cart = await _context.carts
                .Include(c => c.CartItems)
                .ThenInclude(ci => ci.Product)
                .ThenInclude(p => p.Translations)
                .FirstOrDefaultAsync(c => c.UserId == userId);

            if (cart == null)
                return Ok(new { total = 0, items = new List<object>() });

            var resultItems = cart.CartItems.Select(i => new
            {
                i.Id,
                i.ProductId,
                ProductName = i.Product.Translations.FirstOrDefault()?.Name,
                i.Quantity,
                Price = i.Product.Price,
                Total = i.Product.Price * i.Quantity
            });

            var totalPrice = resultItems.Sum(i => i.Total);

            return Ok(new
            {
                cart.Id,
                cart.UserId,
                Items = resultItems,
                TotalPrice = totalPrice
            });
        }

        // ==================================================
        // CLEAR CART
        // ==================================================
        [HttpDelete("clear/{userId}")]
        public async Task<IActionResult> ClearCart(string userId)
        {
            var cart = await _context.carts
                .Include(c => c.CartItems)
                .FirstOrDefaultAsync(c => c.UserId == userId);

            if (cart == null)
                return NotFound();

            _context.CartItems.RemoveRange(cart.CartItems);

            await _context.SaveChangesAsync();

            return Ok(new { message = "Cart cleared" });
        }
    }
}
