using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Talabat.Core.Models
{
    public class CustomerBasket
    {
        // As Redis is a key-value per so busket will contain Key => GUID id , Value => list of items
        public string Id { get; set; }
        public List<BasketItem> Items { get; set; }

        public string? PaymentIntentId { get; set; }
        public string? ClientSecrete { get; set; }
        public int? DeliveryMethodId { get; set; }
        public CustomerBasket(string id)
        {
            Id = id;
            Items = new List<BasketItem>();
        }
    }
}
