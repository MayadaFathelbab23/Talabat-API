using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Talabat.Core.Models.Order_Aggregate
{
    public class Order : BaseEntity
    {
        public Order()
        {
            
        }

        public Order(string buyerEmail, Address shippingAddress, DeliveryMethod deliveryMethod, ICollection<OrderItem> orderItems, decimal subTotal , string paymentIntentId)
        {
            BuyerEmail = buyerEmail;
            ShippingAddress = shippingAddress;
            DeliveryMethod = deliveryMethod;
            OrderItems = orderItems;
            SubTotal = subTotal;
            PaymentIntendId = paymentIntentId;
        }

        public string BuyerEmail { get; set; } = null!;
        public DateTimeOffset OrderDate { get; set; } = DateTimeOffset.Now;
        public OrderStatus Status { get; set; } = OrderStatus.Pending;
        public Address ShippingAddress { get; set; } = null!;
        public DeliveryMethod DeliveryMethod { get; set; } // Nav.prop One
        public ICollection<OrderItem> OrderItems { get; set; } = new HashSet<OrderItem>(); // Nav.prop Many
        public decimal SubTotal { get; set; }
        //[NotMapped]
        //public decimal Total { get => DeliveryMethod.Cost + SubTotal; }
        public decimal GetTotal()
            => DeliveryMethod.Cost + SubTotal;
        public string PaymentIntendId { get; set; } 
    }
}
