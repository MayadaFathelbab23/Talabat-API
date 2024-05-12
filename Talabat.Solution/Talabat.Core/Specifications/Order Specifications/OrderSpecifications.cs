using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Models.Order_Aggregate;

namespace Talabat.Core.Specifications.Order_Specifications
{
    public class OrderSpecifications : BaseSpecifications<Order>
    {
        public OrderSpecifications(string email):base( O => O.BuyerEmail == email)
        {
            Includes.Add(o => o.DeliveryMethod);
            Includes.Add(o => o.OrderItems);
            AddOrderByDesc(o => o.OrderDate);
        }
        public OrderSpecifications(string email , int orderId):base(O=> O.BuyerEmail == email && O.Id == orderId)
        {
            Includes.Add(o => o.DeliveryMethod);
            Includes.Add(o => o.OrderItems);
        }
    }
}
