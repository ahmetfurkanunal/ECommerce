using ECommerce.Core.Entities;
using ECommerce.Service.Abstract;
using ECommerce.WebUI.ExtensionMethods;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.WebUI.Controllers
{
    public class FavoritesController : Controller
    {

        //private readonly DatabaseContext _context;

        //public FavoritesController(DatabaseContext context)
        //{
        //    _context = context;
        //}

        private readonly IService<Product> _service;

        public FavoritesController(IService<Product> service)
        {
            _service = service;
        }
        public IActionResult Index()
        {
            var favorites = GetFavorites();
            return View(favorites);
        }

        private List<Product>GetFavorites() 
        { 
        return HttpContext.Session.GetJson<List<Product>>("GetFavorites") ?? [];
        }

        public IActionResult Add(int ProductId)
        {
            var favorites = GetFavorites();
            var product = _service.Find(ProductId);
            if (product != null && !favorites.Any(p => p.Id == ProductId))
            {
                favorites.Add(product);
                HttpContext.Session.SetJson("GetFavorites", favorites);
                Console.WriteLine("Favorilere ürün eklendi: " + product.Name);

            }
            return RedirectToAction("Index");
        }


        public IActionResult Remove(int ProductId)
        {
            var favorites = GetFavorites();
            var product = _service.Find(ProductId);
            if (product != null && favorites.Any(p => p.Id == ProductId))
            {
                favorites.RemoveAll(i => i.Id == product.Id);
                HttpContext.Session.SetJson("GetFavorites", favorites);
                Console.WriteLine("Favorilerden ürün çıkarıldı: " + product.Name);

            }
            return RedirectToAction("Index");
        }
    }
}
