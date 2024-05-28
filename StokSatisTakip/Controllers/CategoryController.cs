using BusinessLayer.Concrete;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Mvc;

namespace StokSatisTakip.Controllers
{
    public class CategoryController : Controller
    { 
        CategoryRepository categoryRepository= new CategoryRepository();
        public IActionResult Details(int id)
        {
            var catdetails = categoryRepository.CategoryDetails(id);
            return View(catdetails);
        }
    }
}
