
using ECommerce.Core.Entities;
using ECommerce.Data;
using ECommerce.Service.Abstract;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.WebUI.ViewComponents

{
    public class Categories : ViewComponent
    {
        //private readonly DatabaseContext _context;

        //public Categories(DatabaseContext context)
        //{
        //    _context = context;
        //}

        private readonly IService<Category> _service;

        public Categories(IService<Category> service)
        {
            _service = service;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            return View(await _service.GetAllAsync(c => c.IsTopMenu && c.IsActive));
        }
    }
}
