using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Models;
using Talabat.Core.Models.Order_Aggregate;

namespace Talabat.Core.Services
{
    public interface IPaymentService
    {
        // Craete Or Update payment intent
        Task<CustomerBasket?> CreateOrUpdatePaymentIntent(string basketId);

        // update payment status
        Task<Order> UpdateOrderStatusToFaildOrSuccessd(string paymentIntentId, bool flag);
    }
}
