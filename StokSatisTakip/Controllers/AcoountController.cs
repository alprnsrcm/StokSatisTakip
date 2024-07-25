using BusinessLayer.Helpers;
using DataAccessLayer.Context;
using EntityLayer.Entities;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace StokSatisTakip.Controllers
{
    public class AccountController : Controller
    {
        private readonly DataContext _context;

        public AccountController(DataContext context)
        {
            _context = context;
        }

        public IActionResult Login()
        {
            return View();
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Login(User data, bool RememberMe)
        {
            var user = await _context.Users
                .FirstOrDefaultAsync(x => x.Email == data.Email);

            if (user != null && HashingHelper.VerifyPassword(data.Password, user.Password))
            {
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                    new Claim(ClaimTypes.Email, user.Email),
                    new Claim(ClaimTypes.Name, user.Name),
                    new Claim(ClaimTypes.Role, user.Role)
                };

                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

                var authProperties = new AuthenticationProperties
                {
                    IsPersistent = RememberMe,
                    ExpiresUtc = RememberMe ? DateTimeOffset.UtcNow.AddDays(7) : DateTimeOffset.UtcNow.AddHours(1)
                };

                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity), authProperties);

                if (user.Role == "Admin")
                {
                    return RedirectToAction("Index", "Main", new { Area = "Admin" });
                }
                else
                {
                    return RedirectToAction("Index", "Main");
                }
            }

            ViewBag.ErrorMessage = "Geçersiz e-mail veya şifre.";
            return View(data);
        }

        [HttpPost]
        public async Task<IActionResult> Register(User data)
        {
            if (_context.Users.Any(x => x.Email == data.Email))
            {
                ViewBag.ErrorMessage = "Bu e-posta adresiyle kayıtlı bir kullanıcı zaten mevcut.";
                return View("Login");
            }

            if (data.Password != data.RePassword)
            {
                ViewBag.ErrorMessage = "Şifreler uyuşmuyor.";
                return View("Login");
            }

            data.Role = "User";
            data.Password = HashingHelper.HashPassword(data.Password);
            data.RePassword = HashingHelper.HashPassword(data.RePassword);

            _context.Users.Add(data);
            await _context.SaveChangesAsync();

            ViewBag.RegisterMessage = "Kayıt işlemi başarılı, giriş yapabilirsiniz.";
            return View("Login");
        }

        [Authorize]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "Main");
        }

        public IActionResult AccessDenied()
        {
            return View();
        }
    }
}
