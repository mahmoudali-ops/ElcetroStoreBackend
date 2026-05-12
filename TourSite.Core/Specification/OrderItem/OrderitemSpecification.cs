using Store.Core.Specifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TourSite.Core.Entities;
using TourSite.Core.Specification.order;

namespace TourSite.Core.Specification.orderitem
{
    public class cartSpecification : BaseSpecifications<OrderItem>
    {
        public cartSpecification(int id) : base(t => t.Id == id)
        {
            applyIncludes();

        }
        public cartSpecification(cartitemSpecParams specParams) :
            base()
        {

            applyIncludes();

            ApplyPag(specParams.pageSize, specParams.pageIndex);
        }

        public void applyIncludes()
        {



        }


    }
}
