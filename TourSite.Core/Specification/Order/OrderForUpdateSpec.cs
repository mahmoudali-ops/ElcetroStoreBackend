using Store.Core.Specifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TourSite.Core.Entities;

namespace TourSite.Core.Specification.order
{
    public class OrderForUpdateSpec : BaseSpecifications<Order>
    {
        public OrderForUpdateSpec(int id) : base(t => t.Id == id)
        {

        }
    }
}
