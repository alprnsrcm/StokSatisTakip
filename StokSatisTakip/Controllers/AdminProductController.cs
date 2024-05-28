using BusinessLayer.Concrete;
using DataAccessLayer.Context;
using EntityLayer.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using StokSatisTakip.Models;
using System.Data;
using System.Web;
using X.PagedList;

namespace StokSatisTakip.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminProductController : Controller
    {
        ProductRepository productRepository = new ProductRepository();
        DataContext database = new DataContext();
        public IActionResult Index(int sayfa=1)
        {

            return View(productRepository.TList("Category").ToPagedList(sayfa,3));
        }
        public IActionResult Create()
        {
            List<SelectListItem> deger1 = (from i in database.Categories.ToList()
                                           select new SelectListItem
                                           {
                                               Text = i.Name,
                                               Value = i.Id.ToString(),
                                           }).ToList();
            ViewBag.ktgr = deger1;
            return View();
        }
        [HttpPost]
        public IActionResult Create(AddImagine p)
        {
            Product w = new Product();
            if (p.Image != null)
            {
                var extension = Path.GetExtension(p.Image.FileName);
                var newimagename = Guid.NewGuid() + extension;
                var location = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Image/", newimagename);
                var stream = new FileStream(location, FileMode.Create);
                p.Image.CopyTo(stream);
                w.Image = newimagename;
            }
            w.Name = p.Name;
            w.Description = p.Description;
            w.Price = p.Price;
            w.Stock = p.Stock;
            w.Popular = p.Popular;
            w.IsApproved = p.IsApproved;
            w.CategoryId = p.CategoryId;

            //string path = Path.Combine("/wwwroot/Image/" + File.FileName);
            //using (var stream = new FileStream(path, FileMode.Create))
            //File.CopyTo(stream);
            //p.Image = File.FileName.ToString();

            productRepository.Insert(w);
            return RedirectToAction("Index");
        }
        public IActionResult Delete(int id)
        {
            var delete = productRepository.GetById(id);
            productRepository.Delete(delete);
            return RedirectToAction("Index");

        }
        public IActionResult Update(int id)
        {
            List<SelectListItem> deger1 = (from i in database.Categories.ToList()
                                           select new SelectListItem
                                           {
                                               Text = i.Name,
                                               Value = i.Id.ToString(),
                                           }).ToList();
            ViewBag.ktgr = deger1;
            var update = productRepository.GetById(id);
            return View(update);
        }
        [HttpPost]
        public IActionResult Update(Product data, AddImagine p)
        {
            var update = productRepository.GetById(data.Id);
            if (p == null)
            {
                update.Name = data.Name;
                update.Description = data.Description;
                update.Price = data.Price;
                update.Stock = data.Stock;
                update.Popular = data.Popular;
                update.IsApproved = data.IsApproved;
                update.Image = p.Image.FileName.ToString();
                update.CategoryId = data.CategoryId;
                productRepository.Update(update);
                return RedirectToAction("Index");
            }
            else
            {
                update.Name = data.Name;
                update.Description = data.Description;
                update.Price = data.Price;
                update.Stock = data.Stock;
                update.Popular = data.Popular;
                update.IsApproved = data.IsApproved;
                update.CategoryId = data.CategoryId;
                productRepository.Update(update);
                return RedirectToAction("Index");
            }
        }
    }
}
