using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TourSite.Core.DTOs.Cart;
using TourSite.Core.DTOs.User;
using TourSite.Core.Entities;

namespace TourSite.Core.Mapping
{
    public class CartProfile:Profile
    {
        public CartProfile()
        {
            CreateMap<Cart, CartDto>();
        }

    }
}
