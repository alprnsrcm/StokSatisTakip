using BusinessLayer.Concrete;
using EntityLayer.Entities;
using Microsoft.AspNetCore.Mvc;

namespace StokSatisTakip.ViewComponents
{
    public class Categories : ViewComponent
    {
        CategoryRepository categoryRepository = new CategoryRepository();
        public IViewComponentResult Invoke()
        {

            var categorylist = categoryRepository.List();
            
            return View(categorylist);
        }
        
    }
}
