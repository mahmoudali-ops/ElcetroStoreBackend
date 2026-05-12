using Store.Core.Specifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TourSite.Core.Entities;

namespace TourSite.Core.Specification.products
{
    public class CategoryWithCountSpecifications : BaseSpecifications<Product>
    {
        public CategoryWithCountSpecifications(ProdcutsSpecParams specParams) :
            base(p => p.IsAvailable == true)
        { }

    }
}
