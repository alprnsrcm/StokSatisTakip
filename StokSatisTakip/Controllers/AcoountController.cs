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
            var bilgiler = await _context.Users
                .FirstOrDefaultAsync(x => x.Email == data.Email && x.Password == data.Password);

            if (bilgiler != null)
            {
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.NameIdentifier, bilgiler.Id.ToString()),
                    new Claim(ClaimTypes.Email, bilgiler.Email),
                    new Claim(ClaimTypes.Name, bilgiler.Name),
                    new Claim(ClaimTypes.Role, bilgiler.Role)
                };

                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

                var authProperties = new AuthenticationProperties
                {
                    IsPersistent = RememberMe,
                    ExpiresUtc = RememberMe ? DateTimeOffset.UtcNow.AddDays(7) : DateTimeOffset.UtcNow.AddHours(1)
                };

                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity), authProperties);

                if (bilgiler.Role == "Admin")
                {
                    return RedirectToAction("Index", "Main", new { Area = "Admin" });
                }
                else
                {
                    return RedirectToAction("Index", "Main");
                }
            }

            ViewBag.ErrorMessage = "Geçersiz kullanıcı adı veya şifre.";
            return View(data);
        }

        [HttpPost]
        public async Task<IActionResult> Register(User data)
        {
            data.Role = "User";

            _context.Users.Add(data);
            await _context.SaveChangesAsync();

            ViewBag.register = "Kayıt işlemi başarılı, giriş yapabilirsiniz.";
            return RedirectToAction("Login");
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
