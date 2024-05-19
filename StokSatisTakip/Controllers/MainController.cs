using BusinessLayer.Concrete;
using Microsoft.AspNetCore.Mvc;
using X.PagedList;

namespace StokSatisTakip.Controllers
{
    public class MainController : Controller
    {
        ProductRepository productRepository = new ProductRepository();
        public IActionResult Index(int sayfa=1)
        {
            return View(productRepository.List().ToPagedList(sayfa,3));
        }
    }
}
