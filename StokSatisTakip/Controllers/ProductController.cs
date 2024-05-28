using BusinessLayer.Concrete;
using Microsoft.AspNetCore.Mvc;

namespace StokSatisTakip.Controllers
{
	public class ProductController : Controller
	{
		ProductRepository productRepository = new ProductRepository();
		public ActionResult Details(int id)
		{
			var details = productRepository.GetById(id);
			return View(details);
		}
		//public PartialViewResult PopularProduct()
		//{
		//    var product = productRepository.GetPopularProduct();
		//    ViewBag.Popular = product;
		//    return PartialView(productRepository.GetPopularProduct());
		//}
		public ActionResult ProductDetails(int id)
		{
			var details = productRepository.GetById(id);
			return View(details);
		}
	}
}
