using System.ComponentModel.DataAnnotations;
using Talabat.APIs.DTOs.IdentityDtos;

namespace Talabat.APIs.DTOs
{
    public class OrderDto
    {
        [Required]
        public string BasketId { get; set; } = null!;
        public int DeliveryMethodId { get; set; }
        [Required]
        public AddressDto ShippingAddress { get; set; } = null!;
    }
}
