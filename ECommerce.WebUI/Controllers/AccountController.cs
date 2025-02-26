using ECommerce.Core.Entities;
using ECommerce.Data;
using ECommerce.Service.Abstract;
using ECommerce.WebUI.Models;
using Microsoft.AspNetCore.Authentication;//login
using Microsoft.AspNetCore.Authorization;//login
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims; //login

namespace ECommerce.WebUI.Controllers
{
    public class Accountcontroller : Controller
    {
    //    private readonly DatabaseContext _context;

    //    public AccountController(DatabaseContext context)
    //    {


    //        _context = context;
    //    }

    private readonly IService<AppUser> _service;
        
        public Accountcontroller(IService<AppUser> service)
        {
            _service = service;
        }
        [Authorize]
        public  async Task<IActionResult> IndexAsync()
        {
            AppUser user = await _service.GetAsync(x => x.UserGuid.ToString() == HttpContext.User.FindFirst("UserGuid").Value);
            if (user == null)
            {
                return NotFound();
            }
            var model = new UserEditViewModel()
            {
                Email = user.Email,
                Id = user.Id,
                Name = user.Name,
                Password = user.Password,
                Phone = user.Phone,
                Surname = user.Surname, 
            };
            return View(model);
        }



        [HttpPost,Authorize]
        public async Task<IActionResult> IndexAsync(UserEditViewModel model)
        {
            if (ModelState.IsValid)
            {

                try
                {
                    AppUser user = await _service.GetAsync(x => x.UserGuid.ToString() == HttpContext.User.FindFirst("UserGuid").Value);
                    if (user is not null)
                    {
                        user.Surname = model.Surname;
                        user.Phone = model.Phone;
                        user.Name = model.Name;
                        user.Password = model.Password;
                        user.Email = model.Email;
                        _service.Update(user);
                        var sonuc = _service.SaveChanges();
                        if (sonuc > 0)
                        {
                            TempData["Message"] = "<div class='alert alert-success'>Your info is updated.</div>";
                            return RedirectToAction("Index");
                        }
                    }
                }

                catch (Exception)
                {
                    ModelState.AddModelError("", "Hata Oluştu!.");
                }

               
            }
            return View(model);
        }


        public IActionResult SıgnIn()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> SıgnInAsync(LoginViewModel loginViewModel)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var account = await _service.GetAsync(x => x.Email == loginViewModel.Email & x.Password == loginViewModel.Password & x.IsActive);
                    if (account == null)
                    {
                        ModelState.AddModelError("", "Giriş Başarısız");
                    }
                    else
                    {
                        var claims = new List<Claim>()
                        {
                            new(ClaimTypes.Name, account.Name),
                            new(ClaimTypes.Role, account.IsAdmin ? "Admin" : "Customer"),
                            new(ClaimTypes.Email, account.Email),
                            new("UserId", account.Id.ToString()),
                            new("UserGuid",  account.UserGuid.ToString()),
                        };
                        var userIdentity = new ClaimsIdentity(claims, "Login");
                        ClaimsPrincipal userPrincipal = new ClaimsPrincipal(userIdentity);
                        await HttpContext.SignInAsync(userPrincipal);
                        return Redirect(string.IsNullOrEmpty(loginViewModel.ReturnUrl) ? "/" : loginViewModel.ReturnUrl);
                    }
                }
                catch (Exception hata)
                {

                    ModelState.AddModelError("", "Hata Oluştu");
                }
            }
            return View(loginViewModel);
        }

       
        public IActionResult SıgnUp()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> SıgnUpAsync(AppUser appUser)
        {
            appUser.IsAdmin = false;
            appUser.IsActive = true;
            if (ModelState.IsValid)
            {
                await  _service.AddAsync(appUser);
                await _service.SaveChangesAsync();
                return RedirectToAction(nameof(IndexAsync));
            }
            return View(appUser);
        }

        public async Task<IActionResult> SıgnOutAsync()
        {
            await HttpContext.SignOutAsync();
            return RedirectToAction("SıgnIn");
        }
    }
}
