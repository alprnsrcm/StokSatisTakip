using DataAccessLayer.Context;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using X.PagedList;
using X.PagedList.Mvc;

namespace StokSatisTakip.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminSalesController : Controller
    {
        DataContext db=new DataContext();
        public ActionResult Index(int sayfa = 1)
        {
            return View(db.Sales.Include(x=> x.User).Include(x => x.Product).ToList().ToPagedList(sayfa, 6));
        }
    }
}
