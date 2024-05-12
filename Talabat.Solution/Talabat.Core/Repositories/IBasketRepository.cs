using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Models;

namespace Talabat.Core.Repositories
{
    public interface IBasketRepository
    {
        // Creat - Update basket
        public Task<CustomerBasket?> UpdateBasketAsync(CustomerBasket customerBasket);
        // Delete basket
        public Task<bool> DeleteBasketAsync(string basketId);
        // Get basket
        public Task<CustomerBasket?> GetBasketAsync(string basketId);
    }
}
