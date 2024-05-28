using BusinessLayer.Concrete;
using Microsoft.AspNetCore.Mvc;

namespace StokSatisTakip.ViewComponents
{
    public class PopularProduct:ViewComponent
    {
        ProductRepository productRepository= new ProductRepository();
        public IViewComponentResult Invoke()
        {
			
			var product = productRepository.GetPopularProduct();
            ViewBag.Popular = product;
            return View(product);
        }
    }
}
