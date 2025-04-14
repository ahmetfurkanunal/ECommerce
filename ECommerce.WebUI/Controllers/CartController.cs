using ECommerce.Core.Entities;
using ECommerce.Service.Abstract;
using ECommerce.Service.Concrete;
using ECommerce.WebUI.ExtensionMethods;
using ECommerce.WebUI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.WebUI.Controllers
{
    public class CartController : Controller
    {
        private readonly IService<AppUser> _serviceAppUser;
        private readonly IService<Product> _serviceProduct;
        private readonly IService<Adress> _serviceAdress;



        public CartController(IService<Product> serviceProduct, IService<Adress> serviceAdress, IService<AppUser> serviceAppUser)
        {
            _serviceProduct = serviceProduct;
            _serviceAdress = serviceAdress;
            _serviceAppUser = serviceAppUser;
        }
        public IActionResult Index()
        {
            var cart = GetCart();
            var model = new CartViewModel()
            {
                CartLines = cart.CartLines,
                TotalPrice = cart.TotalPrice()
            };

            return View(model);
        }

        public IActionResult Add(int ProductId,int quantity = 1  )
        {

           var product = _serviceProduct.Find(ProductId);

            if (product != null) {
                var cart = GetCart();
                cart.AddProduct(product,quantity);
                HttpContext.Session.SetJson("Cart",cart);
                return Redirect(Request.Headers["Referer"].ToString()); 
            }

            return RedirectToAction("Index");
        }    
            public IActionResult Update(int ProductId,int quantity = 1  )
        {

           var product = _serviceProduct.Find(ProductId);
            if (product != null) {
                var cart = GetCart();
                cart.UpdateProduct(product,quantity);
                HttpContext.Session.SetJson("Cart",cart);
            }
            return RedirectToAction("Index");
        }    
        public IActionResult Remove(int ProductId  )
        {

           var product = _serviceProduct.Find(ProductId);
            if (product != null) {
                var cart = GetCart();
                cart.RemoveProduct(product);
                HttpContext.Session.SetJson("Cart",cart);
            }
            return RedirectToAction("Index");
        }
        [Authorize]
        public async Task<IActionResult> CheckoutAsync()
        {
            var cart = GetCart();
            var appUser = await _serviceAppUser.GetAsync(x => x.UserGuid.ToString() == HttpContext.User.FindFirst("UserGuid").Value);
            if (appUser == null)
            {
                return RedirectToAction("SignIn", "Account");
            }

            var addresses = await _serviceAdress.GetAllAsync(a => a.AppUserId == appUser.Id && a.IsActive);
            var model = new CheckoutViewModel()
            {
                CartProducts = cart.CartLines,
                TotalPrice = cart.TotalPrice(),
                Adresses = addresses
            };

            return View(model);
        }

        private CartService GetCart()
        {
            return HttpContext.Session.GetJson<CartService>("Cart") ?? new CartService();
        }


    }
}
