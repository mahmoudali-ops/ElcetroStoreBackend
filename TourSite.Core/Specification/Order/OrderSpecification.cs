using Store.Core.Specifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TourSite.Core.Entities;
using TourSite.Core.Specification.order;

namespace TourSite.Core.Specification.order
{
    public class OrderitemSpecification : BaseSpecifications<Order>
    {
        public OrderitemSpecification(int id) : base(t => t.Id == id)
        {
            applyIncludes();

        }
        public OrderitemSpecification(OrderSpecParams specParams) :
            base()
        {

            applyIncludes();

            ApplyPag(specParams.pageSize, specParams.pageIndex);
        }

        public void applyIncludes()
        {
            Includes.Add(t => t.OrderItems);



        }


    }
}
