using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TourSite.Core.Entities;

namespace TourSite.Core.DTOs.Order
{
    public class OrderCreateDto
    {
        public int Id { get; set; }

        public string UserId { get; set; }


        public DateTime OrderDate { get; set; }

        public decimal TotalPrice { get; set; }

        public OrderStatus Status { get; set; }

        public PaymentMethod PaymentMethod { get; set; }

        public ICollection<OrderItemDto> OrderItems { get; set; }
    }
}
