using Basket.API.Model;
using Newtonsoft.Json;
using Serilog;
using StackExchange.Redis;
using System.Threading.Tasks;

namespace Basket.API.Repositories
{
    public class RedisBasketRepository : IBasketRepository
    {
        private readonly ConnectionMultiplexer _connectionMultiplexer;
        private readonly IDatabase _database;

        public RedisBasketRepository(ConnectionMultiplexer connectionMultiplexer)
        {
            _connectionMultiplexer = connectionMultiplexer;
            _database = _connectionMultiplexer.GetDatabase();
        }

        public async Task<CustomerBasket> GetBasketAsync(string customerId)
        {
            var customerBasket = await _database.StringGetAsync(customerId);

            if (customerBasket.IsNullOrEmpty)
                return null;

            return JsonConvert.DeserializeObject<CustomerBasket>(customerBasket);
        }

        public async Task<CustomerBasket> UpdateBasketAsync(CustomerBasket customerBasket)
        {
            var data = JsonConvert.SerializeObject(customerBasket);
            if (!await _database.StringSetAsync(customerBasket.BuyerId, data))
            {
                Log.Error("Problem persisting the item {0}", data);
            }

            Log.Information("Basket item persisted successfully {}", data);

            return await GetBasketAsync(customerBasket.BuyerId);
        }

        public async Task DeleteBasketAsync(string customerId)
        {
            await _database.KeyDeleteAsync(customerId);
        }
    }
}
