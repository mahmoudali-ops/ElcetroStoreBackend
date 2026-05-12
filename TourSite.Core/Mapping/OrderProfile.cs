using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TourSite.Core.DTOs.Order;
using TourSite.Core.DTOs.Product;
using TourSite.Core.Entities;

namespace TourSite.Core.Mapping
{
    public class OrderProfile : Profile
    {
        public OrderProfile()
        {
            CreateMap<Order, OrderDto>();

            CreateMap<OrderCreateDto, Order>();

        }
    }
}
