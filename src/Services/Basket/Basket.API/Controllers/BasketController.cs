using System;
using System.Net;
using System.Threading.Tasks;
using Basket.API.IntegrationEvents.Events;
using Basket.API.Model;
using Basket.API.Repositories;
using EventBus;
using Microsoft.AspNetCore.Mvc;
using Serilog;

namespace Basket.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BasketController : ControllerBase
    {
        private readonly IBasketRepository _basketRespository;
        private readonly IEventBus _eventBus;

        public BasketController(IBasketRepository basketRespository,
            IEventBus eventBus)
        {
            _basketRespository = basketRespository;
            _eventBus = eventBus;
        }

        [HttpGet("{userId}")]
        [ProducesResponseType(typeof(CustomerBasket), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<CustomerBasket>> GetBasketByIdAsync(
            string userId)
        {
            var basket = await _basketRespository.GetBasketAsync(userId);

            return Ok(basket ?? new CustomerBasket(userId));
        }

        [HttpPost]
        [ProducesResponseType(typeof(CustomerBasket), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<CustomerBasket>> UpdateBasketAsync(
            [FromBody]CustomerBasket value)
        {
            return Ok(await _basketRespository.UpdateBasketAsync(value));
        }

        [HttpDelete("{userId}")]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.OK)]
        public async Task DeleteBasketAsync(string userId)
        {
            await _basketRespository.DeleteBasketAsync(userId);
        }

        [Route("checkout")]
        [HttpPost]
        [ProducesResponseType((int)HttpStatusCode.Accepted)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<ActionResult> CheckoutAsync(
            [FromBody]BasketCheckout basketCheckout)
        {
            var basket = await _basketRespository.GetBasketAsync(basketCheckout.Buyer);

            if (basket == null)
            {
                return BadRequest();
            }

            var eventMessage = new UserCheckoutAcceptedIntegrationEvent(
                basketCheckout.Buyer, "", basketCheckout.City, basketCheckout.Street,
                basketCheckout.State, basketCheckout.Country, basketCheckout.ZipCode, 
                basketCheckout.CardNumber, basketCheckout.CardHolderName,
                basketCheckout.CardExpiration, basketCheckout.CardSecurityNumber, 
                basketCheckout.CardTypeId, basketCheckout.Buyer, basketCheckout.RequestId, basket);

            // Once basket is checkout, sends an integration event to
            // ordering.api to convert basket to order and proceeds with
            // order creation process
            try
            {
                _eventBus.Publish(eventMessage);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "ERROR Publishing integration event: {IntegrationEventId} from {AppName}",
                    eventMessage.Id, Program.AppName);

                throw;
            }

            return Accepted();
        }
    }
}