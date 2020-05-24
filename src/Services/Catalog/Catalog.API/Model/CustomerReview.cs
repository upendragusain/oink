using System;

namespace Catalog.API.Model
{
    public class CustomerReview
    {
        public int Id { get; set; }

        public string Review { get; set; }

        public string CustomerName { get; set; }

        public Uri CustomerPicture { get; set; }
    }
}
