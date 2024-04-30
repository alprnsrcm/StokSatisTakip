using BusinessLayer.Concrete;
using EntityLayer.Entities;
using Microsoft.AspNetCore.Mvc;

namespace StokSatisTakip.Controllers
{
    public class AdminCategory : Controller
    {
        CategoryRepository categoryRepository = new CategoryRepository();
        public IActionResult Index()
        {
            return View(categoryRepository.List());
        }
        public IActionResult Create()
        {
            return View();
        }
        [ValidateAntiForgeryToken]
        [HttpPost]
        public IActionResult Create(Category p)
        {
            if (ModelState.IsValid)
            {
                categoryRepository.Insert(p);
                return RedirectToAction("Index");
            }
            ModelState.AddModelError("","Bir Hata Oluşstu");
            return View();
        }
    }
}
