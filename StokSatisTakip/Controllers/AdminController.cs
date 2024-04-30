using Microsoft.AspNetCore.Mvc;

namespace StokSatisTakip.Controllers
{
	public class AdminController : Controller
	{
		public IActionResult Index()
		{
			return View();
		}
	}
}
