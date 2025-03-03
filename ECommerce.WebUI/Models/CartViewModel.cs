using ECommerce.Core.Entities;

namespace ECommerce.WebUI.Models
{
    public class CartViewModel
    {
        public List<CartLine>? CartLines { get; set; }

        public decimal TotalPrice { get; set; }
    }
}
