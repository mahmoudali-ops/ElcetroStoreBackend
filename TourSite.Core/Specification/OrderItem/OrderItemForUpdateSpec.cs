using Store.Core.Specifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TourSite.Core.Entities;

namespace TourSite.Core.Specification.orderitem
{
    public class cartForUpdateSpec : BaseSpecifications<OrderItem>
    {
        public cartForUpdateSpec(int id) : base(t => t.Id == id)
        {

        }
    }
}
