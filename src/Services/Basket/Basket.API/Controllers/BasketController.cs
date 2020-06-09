using System.Net;
using System.Threading.Tasks;
using Basket.API.Model;
using Basket.API.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace Basket.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BasketController : ControllerBase
    {
        private readonly IBasketRepository _basketRespository;
        public BasketController(IBasketRepository basketRespository)
        {
            _basketRespository = basketRespository;
        }

        [HttpGet("{customerId}")]
        [ProducesResponseType(typeof(CustomerBasket), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<CustomerBasket>> GetBasketByIdAsync(string customerId)
        {
            var basket = await _basketRespository.GetBasketAsync(customerId);

            return Ok(basket ?? new CustomerBasket(customerId));
        }

        [HttpPost]
        [ProducesResponseType(typeof(CustomerBasket), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<CustomerBasket>> UpdateBasketAsync([FromBody]CustomerBasket value)
        {
            return Ok(await _basketRespository.UpdateBasketAsync(value));
        }

        [HttpDelete("{customerId}")]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.OK)]
        public async Task DeleteBasketAsync(string customerId)
        {
            await _basketRespository.DeleteBasketAsync(customerId);
        }
    }
}