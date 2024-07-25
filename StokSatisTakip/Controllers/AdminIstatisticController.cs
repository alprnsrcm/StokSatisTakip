using DataAccessLayer.Context;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace StokSatisTakip.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminIstatisticController : Controller
    {
        DataContext db = new DataContext();
        public IActionResult Index()
        {
            var satis = db.Sales.Count();
            ViewBag.satis = satis;

			var urun = db.Products.Count();
			ViewBag.urun = urun;

			var kategori = db.Categories.Count();
			ViewBag.kategori = kategori;

			var sepet = db.Carts.Count();
			ViewBag.sepet = sepet;

			var kullanici = db.Users.Where(x => x.Role == "User").Count();
			ViewBag.kullanici = kullanici;
			return View();
        }
    }
}
