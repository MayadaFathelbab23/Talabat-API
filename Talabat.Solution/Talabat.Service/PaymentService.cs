using Microsoft.Extensions.Configuration;
using Stripe;
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
using Product = Talabat.Core.Models.Product;

namespace Talabat.Service
{
    public class PaymentService : IPaymentService
    {
        private readonly IConfiguration _configuration;
        private readonly IBasketRepository _basketRepository;
        private readonly IUnitOfWork _unitOfWork;

        public PaymentService(IConfiguration configuration , 
            IBasketRepository basketRepository ,
            IUnitOfWork unitOfWork)
        {
            _configuration = configuration;
            _basketRepository = basketRepository;
            _unitOfWork = unitOfWork;
        }
        public async Task<CustomerBasket?> CreateOrUpdatePaymentIntent(string basketId)
        {
            // Secret Key
            StripeConfiguration.ApiKey = _configuration["Stripe:Secretkey"];

            // Get Basket
            var Basket = await _basketRepository.GetBasketAsync(basketId);
            if (Basket is null) return null;

            // Calculate subtotal from basket items
            if(Basket.Items.Count > 0)
            {
                foreach (var item in Basket.Items)
                {
                    var product = await _unitOfWork.Repository<Product>().GetByIdAsync(item.Id);
                    if(product.Price != item.Price)
                        item.Price = product.Price;
                }
            }
            var subTotal = Basket.Items.Sum(item => item.Price * item.Quantity);

            // Calculate DeliveryMethod Cost
            var shippingCost = 0M;
            if (Basket.DeliveryMethodId.HasValue)
            {
                var DeliveryMethod = await _unitOfWork.Repository<DeliveryMethod>().GetByIdAsync(Basket.DeliveryMethodId.Value);
                shippingCost = DeliveryMethod.Cost;
            }

            // Create Payment Intent
            var Service = new PaymentIntentService();
            PaymentIntent paymentIntent;
            if (string.IsNullOrEmpty(Basket.PaymentIntentId)) //create new intent
            {
                var createOptions = new PaymentIntentCreateOptions() {
                    Amount = (long)subTotal * 100 + (long)shippingCost * 100,
                    Currency = "usd",
                    PaymentMethodTypes = new List<string>() { "card" }
                };
                paymentIntent = await Service.CreateAsync(createOptions);
            }
            else //update intent
            {
                var updateOptions = new PaymentIntentUpdateOptions()
                {
                    Amount = (long)subTotal * 100 + (long)shippingCost * 100
                };
                paymentIntent = await Service.UpdateAsync(Basket.PaymentIntentId , updateOptions);
            }

            // Update Basket object
            Basket.PaymentIntentId = paymentIntent.Id;
            Basket.ClientSecrete = paymentIntent.ClientSecret;

            // Update Basket data
            Basket = await _basketRepository.UpdateBasketAsync(Basket);
            return Basket;
        }

        public async Task<Order> UpdateOrderStatusToFaildOrSuccessd(string paymentIntentId, bool flag)
        {
            // get order by payment intent is
            var spec = new OrderWithPaymenyIntentSpec(paymentIntentId);
            var order = await _unitOfWork.Repository<Order>().GetEntityWithSpecAsync(spec);
            if (flag)
                order.Status = OrderStatus.PaymentRecieved;
            else
                order.Status = OrderStatus.PaymentFaild;
            _unitOfWork.Repository<Order>().Update(order);
            await _unitOfWork.CompleteAsync();
            return order;
        }
    }
}
