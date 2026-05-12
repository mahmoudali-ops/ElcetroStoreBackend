using Store.Core.Specifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TourSite.Core.Entities;

namespace TourSite.Core.Specification.category
{
    public class CategorySpecification : BaseSpecifications<Category>
    {
        public CategorySpecification(int id) : base(t => t.Id == id)
        {
            applyIncludes();

        }
        public CategorySpecification(CategorypecParams specParams) :
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
