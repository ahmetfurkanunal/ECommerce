using ECommerce.Core.Entities;
using ECommerce.Service.Abstract;
using ECommerce.Service.Concrete;
using ECommerce.WebUI.ExtensionMethods;
using ECommerce.WebUI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.WebUI.Controllers
{
    public class CartController : Controller
    {
        private readonly IService<Order> _serviceOrder;
        private readonly IService<AppUser> _serviceAppUser;
        private readonly IService<Product> _serviceProduct;
        private readonly IService<Adress> _serviceAdress;
        



        public CartController(IService<Product> serviceProduct, IService<Adress> serviceAdress, IService<AppUser> serviceAppUser, IService<Order> serviceOrder)
        {
            _serviceProduct = serviceProduct;
            _serviceAdress = serviceAdress;
            _serviceAppUser = serviceAppUser;
            _serviceOrder = serviceOrder;
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
        public async Task<IActionResult> Checkout()
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

        [Authorize, HttpPost]
        public async Task<IActionResult> Checkout(string CardNumber, string CardMonth, string CardYear, string CVV, string Adresses, string BillingAdress)
        {
            var cart = GetCart();
            var appUser = await _serviceAppUser.GetAsync(x => x.UserGuid.ToString() == HttpContext.User.FindFirst("UserGuid").Value);
            if (appUser == null)
                return RedirectToAction("SignIn", "Account");

            var addresses = await _serviceAdress.GetAllAsync(a => a.AppUserId == appUser.Id && a.IsActive);
            var model = new CheckoutViewModel()
            {
                CartProducts = cart.CartLines,
                TotalPrice = cart.TotalPrice(),
                Adresses = addresses
            };

            if (string.IsNullOrWhiteSpace(CardNumber) || string.IsNullOrWhiteSpace(CardMonth) ||
                string.IsNullOrWhiteSpace(CardYear) || string.IsNullOrWhiteSpace(CVV) ||
                string.IsNullOrWhiteSpace(Adresses) || string.IsNullOrWhiteSpace(BillingAdress))
                return View(model);

            var teslimatAdresi = addresses.FirstOrDefault(a => a.AdressGuid.ToString() == Adresses);
            var faturaAdresi = addresses.FirstOrDefault(a => a.AdressGuid.ToString() == BillingAdress);

            // ✅ Sipariş oluşturma
            Console.WriteLine("🚨 CHECKOUT POST BAŞLADI");

            try
            {
                Console.WriteLine("_serviceOrder null mu? " + (_serviceOrder == null));
                Console.WriteLine("cart null mu? " + (cart == null));
                Console.WriteLine("appUser null mu? " + (appUser == null));

                var siparis = new Order
                {
                    AppUserId = appUser.Id,
                    BillingAddress = "deneme",
                    DeliveryAddress = "deneme",
                    CustomerId = appUser.UserGuid.ToString(),
                    OrderDate = DateTime.Now,
                    TotalPrice = cart.TotalPrice(),
                    OrderNumber = Guid.NewGuid().ToString(),
                    OrderLines = new List<OrderLine>
        {
            new OrderLine
            {
                ProductId = 1,
                Quantity = 1,
                UnitPrice = 10
            }
        }
                };

                await _serviceOrder.AddAsync(siparis);
                var sonuc = await _serviceOrder.SaveChangesAsync();

                Console.WriteLine("✅ Save sonucu: " + sonuc);
                HttpContext.Session.Remove("Cart");
                return RedirectToAction("Thanks");
            }
            catch (Exception ex)
            {
                Console.WriteLine("❌ HATA: " + ex.Message);
                Console.WriteLine("❌ STACK: " + ex.StackTrace);
                return View();
            }
        }



        public IActionResult Thanks()
        {
            return View();
        }
        private CartService GetCart()
        {
            return HttpContext.Session.GetJson<CartService>("Cart") ?? new CartService();
        }


    }
}
