using ECommerce.Core.Entities;

namespace ECommerce.WebUI.Controllers
{
    public class ProductDetailsViewModel
    {
        public Product? Product { get; set; }

        public IEnumerable<Product>? RelatedProducts { get; set; }
    }
}
