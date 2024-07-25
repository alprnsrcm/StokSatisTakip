using DataAccessLayer.Context;
using Microsoft.AspNetCore.Mvc;

namespace StokSatisTakip.ViewComponents
{
    public class StockYellow : ViewComponent
    {
        DataContext db = new DataContext();
        public IViewComponentResult Invoke()
        {

            var kritik = db.Products.Where(x => x.Stock == 50).ToList();

            return View(kritik);
        }
    }
}
