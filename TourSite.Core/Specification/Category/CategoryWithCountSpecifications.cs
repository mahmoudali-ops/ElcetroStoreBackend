using Store.Core.Specifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TourSite.Core.Entities;

namespace TourSite.Core.Specification.category
{
    public class CategoryWithCountSpecifications : BaseSpecifications<Category>
    {
        public CategoryWithCountSpecifications(CategorypecParams specParams) :
            base()
        { }

    }
}
