using BusinessLayer.Concrete;
using EntityLayer.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace StokSatisTakip.Controllers
{
    [Authorize(Roles ="Admin")]
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
            //if (!ModelState.IsValid)
            //{
            //    return View("Create");
            //}
            categoryRepository.Insert(p);
            return RedirectToAction("Index");
        }
        public IActionResult Delete(int id)
        {
            var delete = categoryRepository.GetById(id);
            categoryRepository.Delete(delete);
            return RedirectToAction("Index");
        }

        public IActionResult Update(int id)
        {
            var update = categoryRepository.GetById(id);
            return View(update);
        }
        [ValidateAntiForgeryToken]
        [HttpPost]
        public IActionResult Update(Category p)
        {
            var update = categoryRepository.GetById(p.Id);
            update.Name = p.Name;
            update.Description = p.Description;
            categoryRepository.Update(update);
            return RedirectToAction("Index");
        }
    }
}
