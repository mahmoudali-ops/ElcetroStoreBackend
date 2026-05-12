using Store.Core.Specifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TourSite.Core.Entities;
using TourSite.Core.Specification.order;

namespace TourSite.Core.Specification.cartItem
{
    public class orderitemWithCountSpecifications : BaseSpecifications<CartItem>
    {
        public orderitemWithCountSpecifications(cartitemSpecParams specParams) :
            base()
        { }

    }
}
