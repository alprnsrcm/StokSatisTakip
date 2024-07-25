using BusinessLayer.Concrete;
using DataAccessLayer.Context;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace StokSatisTakip.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        DataContext db = new DataContext();
        UserRepository userRepository = new UserRepository();
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult UserList()
        {
            var user = db.Users.Where(x => x.Role == "User").ToList();
            return View(user);
        }
        public IActionResult Delete(int id)
        {
            var delete = userRepository.GetById(id);
            userRepository.Delete(delete);
            return RedirectToAction("UserList");
        }
    }
}
