using Talabat.Core.Models.Order_Aggregate;

namespace Talabat.APIs.DTOs
{
    public class OrderToReturnDto
    {
        public int Id { get; set; }
        public string BuyerEmail { get; set; } = null!;
        public DateTimeOffset OrderDate { get; set; }
        public string Status { get; set; }
        public Address ShippingAddress { get; set; } = null!;
        public string DeliveryMethod { get; set; } // name of DeliveryMethod
        public decimal DeliveryMethodCost { get; set; } // cost of DeliveryMethod
        public ICollection<OrderItemDto> OrderItems { get; set; } = new HashSet<OrderItemDto>(); 
        public decimal SubTotal { get; set; }
        public decimal Total { get; set; }
        public string PaymentIntendId { get; set; } 
    }
}
