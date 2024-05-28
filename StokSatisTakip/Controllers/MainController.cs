using BusinessLayer.Concrete;
using EntityLayer.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using X.PagedList;

namespace StokSatisTakip.Controllers
{
    [Authorize]
    public class MainController : Controller
    {
        ProductRepository productRepository = new ProductRepository();
        CategoryRepository categoryRepository = new CategoryRepository();
        public IActionResult Index(int sayfa=1)
        {
			ViewBag.Categories = categoryRepository;
			return View(productRepository.List().ToPagedList(sayfa,3));
        }
		public IActionResult CategoryList()
        {
            var categorylist= new List<Category>();
            return View(categorylist);
        }
	}
}
