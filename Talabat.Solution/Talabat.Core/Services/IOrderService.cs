using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Models.Order_Aggregate;

namespace Talabat.Core.Services
{
    public interface IOrderService
    {
        // Create Order Sign.
        Task<Order?> CreateOrderAsync(string BuyerEmael, string Basket, Address ShippingAddress, int DeliveryMethodId);

        // Get Orders for Specific user
        Task<IReadOnlyList<Order>> GetOrdersForSpecificUserAsync(string BuyerEmail);

        // Get Order by Id for Specific user
        Task<Order> GetOrderByIdForSpecificUserAsync(string BuyerEmail , int orderId);

        // Get All DelliverMethods
        Task<IReadOnlyList<DeliveryMethod>> GetDeliveryMethodsAsync();

    }
}
