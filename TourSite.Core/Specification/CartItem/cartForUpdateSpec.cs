using Store.Core.Specifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TourSite.Core.Entities;

namespace TourSite.Core.Specification.cartItem
{
    public class cartForUpdateSpec : BaseSpecifications<CartItem>
    {
        public cartForUpdateSpec(int id) : base(t => t.Id == id)
        {

        }
    }
}
