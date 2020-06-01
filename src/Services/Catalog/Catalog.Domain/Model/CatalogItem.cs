using System.Collections.Generic;

namespace Catalog.Domain.Model
{
    public abstract class CatalogItem
    {
        public CatalogItem()
        {
            Department = new DepartmentType();
            Categories = new List<CategoryType>();
            Media = new List<MediaContent>();
            Reviews = new List<CustomerReview>();
        }

        public string Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public decimal Price { get; set; }

        public DepartmentType Department { get; set; }

        public List<CategoryType> Categories { get; set; }

        public List<MediaContent> Media { get; set; }

        public List<CustomerReview> Reviews { get; set; }
    }
}
