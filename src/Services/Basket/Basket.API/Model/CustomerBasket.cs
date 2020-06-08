using System.Collections.Generic;

namespace Basket.API.Model
{
    public class CustomerBasket
    {
        //Deserialization of reference types 
        //without parameterless constructor is not supported. Type 'Basket.API.Model.CustomerBasket'
        public CustomerBasket()
        {

        }

        public CustomerBasket(string buyerId)
        {
            BuyerId = buyerId;
        }

        public string BuyerId { get; set; }

        public List<BasketItem> Items { get; set; }
    }
}
