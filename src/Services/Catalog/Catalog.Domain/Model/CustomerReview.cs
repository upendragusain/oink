using System;

namespace Catalog.Domain.Model
{
    public class CustomerReview
    {
        public int Id { get; set; }

        public string Review { get; set; }

        public string CustomerName { get; set; }

        public Uri CustomerPicture { get; set; }

        public DateTime ReviewDate { get; set; }
    }
}
