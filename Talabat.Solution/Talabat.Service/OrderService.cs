using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core;
using Talabat.Core.Models;
using Talabat.Core.Models.Order_Aggregate;
using Talabat.Core.Repositories;
using Talabat.Core.Services;
using Talabat.Core.Specifications.Order_Specifications;

namespace Talabat.Service
{
    public class OrderService : IOrderService
    {
        private readonly IBasketRepository _basketRepo;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IPaymentService _paymentService;

        public OrderService(IBasketRepository basketRepo , 
            IUnitOfWork unitOfWork,
            IPaymentService paymentService)
        {
            _basketRepo = basketRepo;
            _unitOfWork = unitOfWork;
            _paymentService = paymentService;
        }
        public async Task<Order?> CreateOrderAsync(string BuyerEmael, string BasketId, Address ShippingAddress, int DeliveryMethodId)
        {
            // Get Basket from basket Repo
            var Basket = await _basketRepo.GetBasketAsync(BasketId);

            // From basket items , get product
            var orderItems = new List<OrderItem>();
            if(Basket?.Items?.Count > 0)
            {
                foreach(var item in Basket.Items)
                {
                    var product = await _unitOfWork.Repository<Product>().GetByIdAsync(item.Id);
                    var productOrderItem = new ProductItemOrdered(product.Id, product.Name, product.PictureUrl);
                    var OrderItem = new OrderItem(productOrderItem , product.Price , item.Quantity);
                    orderItems.Add(OrderItem);
                }
            }

            //Calculate subtotal
            var subTotal = orderItems.Sum(item => item.Price *  item.Quantity);

            // Get DeliveryMethod from DeliveryMethod Repo
            var deliverMethod = await _unitOfWork.Repository<DeliveryMethod>().GetByIdAsync(DeliveryMethodId);

            // Create Order Object
            //** Validate if order exists with same paymentIntentId
            var spec = new OrderWithPaymenyIntentSpec(Basket.PaymentIntentId);
            var ExOrder = await _unitOfWork.Repository<Order>().GetEntityWithSpecAsync(spec);
            if(ExOrder is not null)
            {
                // delete order
                _unitOfWork.Repository<Order>().Delete(ExOrder);
                // update basket paymentintent
                await _paymentService.CreateOrUpdatePaymentIntent(BasketId);
            }
            var Order = new Order(BuyerEmael, ShippingAddress, deliverMethod, orderItems, subTotal , Basket.PaymentIntentId);

            //Add Order Locally
           await _unitOfWork.Repository<Order>().AddAsync(Order);

            // Save Order To Database
            var result = await _unitOfWork.CompleteAsync();
            return result <= 0 ? null : Order;
            
        }

        public async Task<IReadOnlyList<DeliveryMethod>> GetDeliveryMethodsAsync()
        => await _unitOfWork.Repository<DeliveryMethod>().GetAllAsync();

        public Task<Order> GetOrderByIdForSpecificUserAsync(string BuyerEmail, int orderId)
        {
            var spec = new OrderSpecifications(BuyerEmail, orderId);
            var order = _unitOfWork.Repository<Order>().GetEntityWithSpecAsync(spec);
            return order;
        }

        public Task<IReadOnlyList<Order>> GetOrdersForSpecificUserAsync(string BuyerEmail)
        {
            var spec = new OrderSpecifications(BuyerEmail);
            var orders = _unitOfWork.Repository<Order>().GetAllWithSpecAsync(spec);
            return orders;
        }
    }
}
