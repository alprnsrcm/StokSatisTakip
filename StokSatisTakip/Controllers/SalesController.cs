using System.Security.Claims;
using BusinessLayer.Extensions;
using DataAccessLayer.Context;
using EntityLayer.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using X.PagedList;
using X.PagedList.Mvc.Core;

namespace StokSatisTakip.Controllers
{
    public class SalesController : Controller
    {
        DataContext db = new DataContext();

        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly ClaimsPrincipal _user;
        public SalesController(IHttpContextAccessor httpContextAccessor)
        {
            this.httpContextAccessor = httpContextAccessor;
            _user = httpContextAccessor.HttpContext.User;
        }

        public IActionResult Index(int sayfa= 1)
        {
            if (User.Identity.IsAuthenticated)
            {
                var kullanici = db.Users.FirstOrDefault(x => x.Email == _user.GetLoggedInEmail());
                var model = db.Sales.Include(x=>x.Product).Where(x => x.UserId == kullanici.Id).ToList().ToPagedList(sayfa, 5);
                return View(model);
            }
            return NotFound();
        }

        public ActionResult Buy(int id)
        {
            var model = db.Carts.Include(x => x.Product).FirstOrDefault(x => x.Id == id);

            return View(model);
        }

        [HttpPost]

        public ActionResult Buy2(int id)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var model = db.Carts.FirstOrDefault(x => x.Id == id);

                    var satis = new Sales
                    {
                        UserId = model.UserId,
                        ProductId = model.ProductId,
                        Quantity = model.Quantity,
                        Image = model.Image,
                        Price = model.Price,
                        Date = model.Date,

                    };

                    db.Carts.Remove(model);
                    db.Sales.Add(satis);
                    db.SaveChanges();
                    ViewBag.islem = "Satın Alma İşlemi Başarılı Bir Şekilde Gerçekleşmiştir";
                }
            }
            catch (Exception)
            {

                ViewBag.islem = "Satın Alma İşlemi Başarısız ";
            }

            return View("islem");

        }
        public ActionResult BuyAll(decimal? Tutar)
        {
            if (User.Identity.IsAuthenticated)
            {
                var kullanici = db.Users.FirstOrDefault(x => x.Email == _user.GetLoggedInEmail());
                var model = db.Carts.Include(x=>x.Product).Where(x => x.UserId == kullanici.Id).ToList();
                var kid = db.Carts.FirstOrDefault(x => x.UserId == kullanici.Id);
                if (model != null)
                {
                    if (kid == null)
                    {
                        ViewBag.Tutar = "Sepetinizde ürün bulunmamaktadır";
                    }
                    else if (kid != null)
                    {
                        Tutar = db.Carts.Where(x => x.UserId == kid.UserId).Sum(x => x.Product.Price * x.Quantity);
                        ViewBag.Tutar = "Toplam Tutar = " + Tutar + "TL";
                    }
                    return View(model);
                }

                return View();
            }
            return NotFound();
        }
        [HttpPost]
        public ActionResult BuyAll2()
        {
            var kullanici = db.Users.FirstOrDefault(x => x.Email == _user.GetLoggedInEmail());
            var model = db.Carts.Where(x => x.UserId == kullanici.Id).ToList();
            int row = 0;
            foreach (var item in model)
            {
                var satis = new Sales
                {
                    UserId = model[row].UserId,
                    ProductId = model[row].ProductId,
                    Quantity = model[row].Quantity,
                    Price = model[row].Price,
                    Image = model[row].Image,
                    Date = DateTime.Now
                };
                db.Sales.Add(satis);
                db.SaveChanges();
                row++;
            }
            db.Carts.RemoveRange(model);
            db.SaveChanges();
            return RedirectToAction("Index", "Cart");
        }
    }
}
