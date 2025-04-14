using ECommerce.Core.Entities;
using ECommerce.Service.Abstract;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace ECommerce.WebUI.Controllers
{
    [Authorize]
    public class MyAdressesController : Controller
    {
        private readonly IService<AppUser> _serviceAppUser;
        private readonly IService<Adress> _serviceAdress;

        public MyAdressesController(IService<AppUser> service, IService<Adress> serviceAdress)
        {
            _serviceAppUser = service;
            _serviceAdress = serviceAdress;
        }
        public  async Task<IActionResult> Index()
        {
            var appUser = await _serviceAppUser.GetAsync(x => x.UserGuid.ToString() == HttpContext.User.FindFirst("UserGuid").Value);
            if (appUser == null)
            {
                return NotFound("Kullanıcı Datası Bulunamadı! Oturumunuzu Kapatıp Lütfen Tekrar Giriş Yapın!");
            }

            var model = await _serviceAdress.GetAllAsync(u => u.AppUserId == appUser.Id);
            return View(model);

            return View(model);
        }
        public IActionResult Create()
        {
           
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Adress adress)
        {
            try
            {
                var appUser = await _serviceAppUser.GetAsync(x => x.UserGuid.ToString() == HttpContext.User.FindFirst("UserGuid").Value);
                if (appUser != null) { 
                    adress.AppUserId = appUser.Id;
                    _serviceAdress.Add(adress);
                    await _serviceAdress.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
            }
            catch (Exception)
            {
                ModelState.AddModelError("", "Hata Oluştu!");
            }

            ModelState.AddModelError("", "Kayot Başarısız!");
            return View(adress);
        }

    
        public async Task<IActionResult> Edit(string id )
        {
            var appUser = await _serviceAppUser.GetAsync(x => x.UserGuid.ToString() ==
                HttpContext.User.FindFirst("UserGuid").Value);
            if (appUser == null)
            {
                return NotFound("Kullanıcı Datası Bulunamadı! Oturumunuzu Kapatıp Lütfen Tekrar Giriş Yapın!");
            }

            var model = await _serviceAdress.GetAsync(u => u.AdressGuid.ToString() == id && u.AppUserId == appUser.Id);
            if (model == null)
            {
                return NotFound("Adres Bilgisi Bulunamadı!");
            }

            return View(model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, Adress address)
        {
            var appUser = await _serviceAppUser.GetAsync(x => x.UserGuid.ToString() ==
                HttpContext.User.FindFirst("UserGuid").Value);
            if (appUser == null)
            {
                return NotFound("Kullanıcı Datası Bulunamadı! Oturumunuzu Kapatıp Lütfen Tekrar Giriş Yapın!");
            }

            var model = await _serviceAdress.GetAsync(u => u.AdressGuid.ToString() == id && u.AppUserId == appUser.Id);
            if (model == null)
            {
                return NotFound("Adres Bilgisi Bulunamadı!");
            }

            model.Title = address.Title;
            model.District = address.District;
            model.City = address.City;
            model.OpenAdress = address.OpenAdress;
            model.IsDeliveryAdress = address.IsDeliveryAdress;
            model.IsBillingAdress = address.IsBillingAdress;
            model.IsActive = address.IsActive;

            var otherAddresses = await _serviceAdress.GetAllAsync(x => x.AppUserId == appUser.Id && x.Id != model.Id);
            foreach (var otherAddress in otherAddresses)
            {
                otherAddress.IsDeliveryAdress = false;
                otherAddress.IsBillingAdress = false;
                _serviceAdress.Update(otherAddress);
            }

            try
            {
                _serviceAdress.Update(model);
                await _serviceAdress.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            catch (Exception)
            {
                ModelState.AddModelError("", "Hata Oluştu");
            }

            return View(model);

        }
        public async Task<IActionResult> Delete(string id)
        {
            var appUser = await _serviceAppUser.GetAsync(x => x.UserGuid.ToString() ==
                HttpContext.User.FindFirst("UserGuid").Value);
            if (appUser == null)
            {
                return NotFound("Kullanıcı Datası Bulunamadı! Oturumunuzu Kapatıp Lütfen Tekrar Giriş Yapın!");
            }

            var model = await _serviceAdress.GetAsync(u => u.AdressGuid.ToString() == id && u.AppUserId == appUser.Id);
            if (model == null)
            {
                return NotFound("Adres Bilgisi Bulunamadı!");
            }

            return View(model);
          
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(string id,Adress adress)
        {
            var appUser = await _serviceAppUser.GetAsync(x => x.UserGuid.ToString() ==
                HttpContext.User.FindFirst("UserGuid").Value);
            if (appUser == null)
            {
                return NotFound("Kullanıcı Datası Bulunamadı! Oturumunuzu Kapatıp Lütfen Tekrar Giriş Yapın!");
            }

            var model = await _serviceAdress.GetAsync(u => u.AdressGuid.ToString() == id && u.AppUserId == appUser.Id);
            if (model == null)
            {
                return NotFound("Adres Bilgisi Bulunamadı!");
            }

            try
            {
                _serviceAdress.Delete(model);
                await _serviceAdress.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            catch (Exception)
            {

                throw;
            }

            return View(model);
        }

    }
}
