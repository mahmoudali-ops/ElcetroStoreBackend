using Store.Core.Specifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TourSite.Core.Entities;

namespace TourSite.Core.Specification.products
{
    public class OrderSlugSpecification : BaseSpecifications<Product>
    {
        public OrderSlugSpecification(string slug)
        : base(t => t.Slug == slug && t.IsAvailable) // ✅ شرط Slug و IsActive
        {
            applyIncludes();
        }
        //public TourSlugSpecification(string slug, TourSpecParams specParams)
        //: base(t => t.Slug == slug && t.IsActive)
        //{
        //    applyIncludes();
        //    ApplyPag(specParams.pageSize, specParams.pageIndex);
        //}

        public void applyIncludes()
        {
            Includes.Add(t => t.Translations);

            Includes.Add(t => t.Images);
       


        }


    }
}
