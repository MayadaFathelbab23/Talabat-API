using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Models.Order_Aggregate;

namespace Talabat.Core.Specifications.Order_Specifications
{
    public class OrderWithPaymenyIntentSpec : BaseSpecifications<Order>
    {
        public OrderWithPaymenyIntentSpec(string paymentIntentId):base(O=>O.PaymentIntendId == paymentIntentId)
        {
            
        }
    }
}
