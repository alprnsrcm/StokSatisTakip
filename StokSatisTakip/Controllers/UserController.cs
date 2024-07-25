using Microsoft.AspNetCore.Mvc;

namespace StokSatisTakip.Controllers
{
    public class UserController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
