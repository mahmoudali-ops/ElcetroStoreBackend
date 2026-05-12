using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TourSite.Core.DTOs.Cart
{
    public class CartCreateDto
    {
        public int Id { get; set; }

        public string UserId { get; set; }


        public ICollection<CartItemDto> CartItems { get; set; }
    }
}
