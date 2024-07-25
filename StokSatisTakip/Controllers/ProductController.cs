using BusinessLayer.Concrete;
using Microsoft.AspNetCore.Mvc;

namespace StokSatisTakip.Controllers
{
	public class ProductController : Controller
	{
		ProductRepository productRepository = new ProductRepository();
		
		public ActionResult ProductDetails(int id)
		{
			var details = productRepository.GetById(id);
			return View(details);
		}
	}
}
