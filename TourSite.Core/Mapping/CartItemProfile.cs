using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TourSite.Core.DTOs.Cart;
using TourSite.Core.Entities;

namespace TourSite.Core.Mapping
{
    public class CartItemProfile:Profile
    {
        public CartItemProfile()
        {
            CreateMap<CartItem, CartItemDto>();
        }

    }
}
