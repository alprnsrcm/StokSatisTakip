using BusinessLayer.Concrete;
using DataAccessLayer.Context;
using Microsoft.AspNetCore.Mvc;

namespace StokSatisTakip.ViewComponents
{
    
    public class StockCount:ViewComponent
    { 
        DataContext db = new DataContext();
        public IViewComponentResult Invoke()
        {
            if (User.Identity.IsAuthenticated)
            {
                var count = db.Products.Where(x => x.Stock < 50).Count();
                ViewBag.count = count;
                var azalan = db.Products.Where(x => x.Stock == 50).Count();
                ViewBag.azalan = azalan;

            }

            return View();
        }
    }
}
