using Basket.API.Model;
using System.Threading.Tasks;

namespace Basket.API.Repositories
{
    public interface IBasketRepository
    {
        Task<CustomerBasket> GetBasketAsync(string userId);

        Task<CustomerBasket> UpdateBasketAsync(CustomerBasket customerBasket);

        Task DeleteBasketAsync(string userId);

    }
}
