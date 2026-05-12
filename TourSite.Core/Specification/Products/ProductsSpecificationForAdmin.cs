using Store.Core.Specifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TourSite.Core.Entities;

namespace TourSite.Core.Specification.products
{
    public class ProdutsSpecificationForAdmin : BaseSpecifications<Product>
    {

        public ProdutsSpecificationForAdmin(int id) : base(t => t.Id == id)
        {
            applyIncludes();

        }
        public ProdutsSpecificationForAdmin(ProdcutsSpecParams specParams) :
            base()
        {

            applyIncludes();
            ApplyPag(specParams.pageSize, specParams.pageIndex);
        }

        public void applyIncludes()
        {
            Includes.Add(t => t.Translations);

            Includes.Add(t => t.Images);

        }

    }
}
