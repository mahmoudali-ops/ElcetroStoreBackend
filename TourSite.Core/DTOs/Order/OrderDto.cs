using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TourSite.Core.Entities;

namespace TourSite.Core.DTOs.Order
{
    public class OrderDto
    {
        public int Id { get; set; }

        public string UserId { get; set; }

        public DateTime OrderDate { get; set; }

        public decimal TotalPrice { get; set; }

        // 👇 بدل enum خام
        public string Status { get; set; }

        public string PaymentMethod { get; set; }

        public ICollection<OrderItemDto> OrderItems { get; set; }
    }
}
