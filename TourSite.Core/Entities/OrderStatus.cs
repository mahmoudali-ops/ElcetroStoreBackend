using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TourSite.Core.Entities
{
    public enum OrderStatus
    {
        Pending,
        Confirmed,
        Preparing,
        OutForDelivery,
        Delivered,
        Cancelled
    }
}
