using Store.Core.Specifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TourSite.Core.Entities;
using TourSite.Core.Specification.category;

namespace TourSite.Core.Specification.category
{
    public class CategorySpecificationForAdmin : BaseSpecifications<Category>
    {

        public CategorySpecificationForAdmin(int id) : base(t => t.Id == id)
        {
            applyIncludes();

        }
        public CategorySpecificationForAdmin(CategorypecParams specParams) :
            base()
        {

            applyIncludes();
            ApplyPag(specParams.pageSize, specParams.pageIndex);
        }

        public void applyIncludes()
        {
            Includes.Add(t => t.Translations);

            Includes.Add(t => t.Products);

        }

    }
}
