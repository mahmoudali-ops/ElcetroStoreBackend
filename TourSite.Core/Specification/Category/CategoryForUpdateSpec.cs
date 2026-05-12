using Store.Core.Specifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TourSite.Core.Entities;

namespace TourSite.Core.Specification.category
{
    public class CategoryForUpdateSpec : BaseSpecifications<Category>
    {
        public CategoryForUpdateSpec(int id) : base(t => t.Id == id)
        {
            Includes.Add(t => t.Translations);

        }
    }
}
