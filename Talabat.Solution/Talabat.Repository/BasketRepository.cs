using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Models;
using Talabat.Core.Repositories;
using StackExchange.Redis;
using System.Text.Json;

namespace Talabat.Repository
{
    public class BasketRepository : IBasketRepository
    {
        private readonly IDatabase _database;
        public BasketRepository(IConnectionMultiplexer redis)
        {
            _database = redis.GetDatabase();
        }
        public async Task<bool> DeleteBasketAsync(string basketId)
        {
            return await _database.KeyDeleteAsync(basketId);
        }

        public async Task<CustomerBasket?> GetBasketAsync(string basketId)
        {
            var basket =  await _database.StringGetAsync(basketId); // basket is redisValue in json format
            return basket.IsNullOrEmpty ? null : JsonSerializer.Deserialize<CustomerBasket>(basket); // json -> CustomerBasket
        }

        public async Task<CustomerBasket?> UpdateBasketAsync(CustomerBasket customerBasket)
        {
            var createdOrUpdated = await _database.StringSetAsync(customerBasket.Id, JsonSerializer.Serialize<CustomerBasket>(customerBasket), TimeSpan.FromDays(30));
            if (!createdOrUpdated)
                return null;
            return await GetBasketAsync(customerBasket.Id);
        }
    }
}
