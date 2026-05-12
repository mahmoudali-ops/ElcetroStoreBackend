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
    public class cartSpecification : BaseSpecifications<Cart>
    {
        public cartSpecification(int id) : base(t => t.Id == id)
        {
            applyIncludes();

        }
        public cartSpecification(cartSpecParams specParams) :
            base()
        {

            applyIncludes();

            ApplyPag(specParams.pageSize, specParams.pageIndex);
        }

        public void applyIncludes()
        {

            Includes.Add(t => t.CartItems);

        }


    }
}
