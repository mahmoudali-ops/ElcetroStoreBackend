using Store.Core.Specifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TourSite.Core.Entities;

namespace TourSite.Core.Specification.products
{
    public class ProductsSpecification : BaseSpecifications<Product>
    {
        public ProductsSpecification(int id) : base(t => t.Id == id)
        {
            applyIncludes();

        }
        public ProductsSpecification(ProdcutsSpecParams specParams) :
            base(p => p.IsAvailable == true)
        {

            applyIncludes();

            ApplyPag(specParams.pageSize, specParams.pageIndex);
        }

        public void applyIncludes()
        {
            Includes.Add(t => t.Translations);

            Includes.Add(t => t.Images);


            Includes.Add(t => t.Category);
            IncludeStrings.Add("Category.Translations");


        }


    }
}
