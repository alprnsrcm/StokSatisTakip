using BusinessLayer.Concrete;
using Microsoft.AspNetCore.Mvc;

namespace StokSatisTakip.Controllers
{
    public class CategoryController : Controller
    {
        CategoryRepository categoryRepository = new CategoryRepository();
        
        public PartialViewResult CategoryList()
        {
            return PartialView(categoryRepository.List());
        }
       
    }
}
