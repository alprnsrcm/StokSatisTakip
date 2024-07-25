using System.Security.Claims;
using BusinessLayer.Extensions;
using DataAccessLayer.Context;
using EntityLayer.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace StokSatisTakip.Controllers
{
    public class CartController : Controller
    {
        DataContext db = new DataContext();

        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly ClaimsPrincipal _user;
        public CartController(IHttpContextAccessor httpContextAccessor)
        {
            this.httpContextAccessor = httpContextAccessor;
            _user = httpContextAccessor.HttpContext.User;
        }

        public IActionResult Index(decimal? Tutar)
        {
            if (User.Identity.IsAuthenticated)
            {
                var userId = _user.GetLoggedInUserId();
                var kullanici = db.Users.FirstOrDefault(x => x.Id==userId);
                var model = db.Carts.Where(x => x.UserId == kullanici.Id).Include(x=>x.Product).ToList();
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
                        ViewBag.Tutar = "Toplam Tutar =" + ((float?)Tutar) + "TL";
                    }
                    return View(model);
                }
            }
            return NotFound();

        }
        public ActionResult AddCart(int id)
        {
            if (User.Identity.IsAuthenticated)
            {

                var kullaniciadi = _user.GetLoggedInEmail();
                var model = db.Users.FirstOrDefault(x => x.Email == kullaniciadi);
                var u = db.Products.Find(id);
                var sepet = db.Carts.FirstOrDefault(x => x.UserId == model.Id && x.ProductId == id);
                if (model != null)
                {
                    if (sepet != null)
                    {
                        sepet.Quantity++;
                        sepet.Price = u.Price * sepet.Quantity;
                        db.SaveChanges();
                        return RedirectToAction("Index", "Cart");
                    }
                    var s = new Cart
                    {
                        Image = u.Image,
                        UserId = model.Id,
                        ProductId = u.Id,
                        Quantity = 1,
                        Price = u.Price,
                        Date = DateTime.Now


                    };
                    db.Carts.Add(s);
                    db.SaveChanges();
                    return RedirectToAction("Index", "Cart");

                }
                return View();



            }
            return NotFound();
        }

        public ActionResult TotalCount(int? count)
        {
            if (User.Identity.IsAuthenticated)
            {

                var model = db.Users.FirstOrDefault(x => x.Email == _user.GetLoggedInEmail()); 
                count = db.Carts.Where(x => x.UserId == model.Id).Count();
                ViewBag.Count = count;
                if (count == 0)
                {
                    ViewBag.Count = "";
                }

                return PartialView();
            }
            return NotFound();
        }

        public ActionResult arttir(int id)
        {
            var model = db.Carts.Find(id);
            model.Quantity++;
            model.Price = model.Price * model.Quantity;
            db.SaveChanges();
            return RedirectToAction("Index", "Cart");
        }
        public ActionResult azalt(int id)
        {
            var model = db.Carts.Find(id);
            if (model.Quantity == 1)
            {
                db.Carts.Remove(model);
                db.SaveChanges();
            }
            model.Quantity--;
            model.Price = model.Price * model.Quantity;
            db.SaveChanges();
            return RedirectToAction("Index", "Cart");
        }
        public void DinamikMiktar(int id, int miktari)
        {
            var model = db.Carts.Find(id);
            model.Quantity = miktari;
            model.Price = model.Price * model.Quantity;
            db.SaveChanges();

        }

        public ActionResult Delete(int id)
        {
            var sil = db.Carts.Find(id);
            db.Carts.Remove(sil);
            db.SaveChanges();
            return RedirectToAction("Index", "Cart");
        }
        public ActionResult DeleteRange()
        {
            if (User.Identity.IsAuthenticated)
            {
                var kullaniciadi = _user.GetLoggedInEmail;
                var model = db.Users.FirstOrDefault(x => x.Email == _user.GetLoggedInEmail());
                var sil = db.Carts.Where(x => x.UserId == model.Id);
                db.Carts.RemoveRange(sil);
                db.SaveChanges();
                return RedirectToAction("Index", "Cart");
            }
            return NotFound();
        }

    }
}

