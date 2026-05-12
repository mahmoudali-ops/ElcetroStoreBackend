using Store.Core.Specifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TourSite.Core.Entities;
using TourSite.Core.Specification.order;

namespace TourSite.Core.Specification.productiamge
{
    public class productsimageWithCountSpecifications : BaseSpecifications<OrderItem>
    {
        public productsimageWithCountSpecifications(productimageSpecParams specParams) :
            base()
        { }

    }
}
