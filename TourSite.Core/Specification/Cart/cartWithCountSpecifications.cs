using Store.Core.Specifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TourSite.Core.Entities;
using TourSite.Core.Specification.order;

namespace TourSite.Core.Specification.cart
{
    public class cartWithCountSpecifications : BaseSpecifications<OrderItem>
    {
        public cartWithCountSpecifications(cartSpecParams specParams) :
            base()
        { }

    }
}
