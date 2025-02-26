using System.Diagnostics;
using ECommerce.Core.Entities;
using ECommerce.Data;
using ECommerce.Service.Abstract;
using ECommerce.WebUI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.WebUI.Controllers
{
    public class HomeController : Controller
    {
        //private readonly DatabaseContext _context;

        //public HomeController(DatabaseContext context)
        //{
        //    _context = context;
        //}

        private readonly IService<Product> _serviceProduct;
        private readonly IService<Slider> _serviceSlider;
       
        private readonly IService<Contact> _serviceContact;

        public HomeController(IService<Product> serviceProduct, IService<Slider> serviceSlider,  IService<Contact> serviceContact)
        {
            _serviceProduct = serviceProduct;
            _serviceSlider = serviceSlider;
            
            _serviceContact = serviceContact;
        }

        public async Task<IActionResult> Index()
        {
            var model = new HomePageViewModel()
            {
                Sliders = await _serviceSlider.GetAllAsync(),
               
                Products = await _serviceProduct.GetAllAsync(p => p.IsActive && p.IsHome)

            };
            return View(model);
        }
        public IActionResult Privacy()
        {
            return View();
        }

        [Route("AccessDenied")]
        public IActionResult AccessDenied()
        {
            return View();
        }

        public IActionResult ContactUs()
        {
            return View();
        }

        [HttpPost]
        public IActionResult ContactUs(Contact contact)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    _serviceContact.Add(contact);
                    var sonuc = _serviceContact.SaveChanges(); 
                    if (sonuc > 0) {
                        TempData["Message"] = "<div class='alert alert-success'>The message sent.</div>";
                        return RedirectToAction("ContactUs");
                    }
                }
                catch(Exception ) 
                {
                    ModelState.AddModelError("", "Error");
                }
            }
            return View(contact);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
