using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using BasicAuthenticationAspNetCore2.Data;
using BasicAuthenticationAspNetCore2.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;

namespace BasicAuthenticationAspNetCore2.Controllers
{
    public class AccountsController : Controller
    {
        private AppDbContext db;
        public AccountsController(AppDbContext _db)
        {
            db = _db;
        }
        public async Task<IActionResult> Login()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(User model,string returnUrl = "/") 
        {
            if(string.IsNullOrEmpty(model.Username) || string.IsNullOrWhiteSpace(model.Username) 
            || string.IsNullOrEmpty(model.Password) || string.IsNullOrWhiteSpace(model.Password))
                return BadRequest("invalide inputs !");
            var user = db.Users.SingleOrDefault(u => u.Username == model.Username);
            if (user == null || user.Password != model.Password)
                return BadRequest("wrong informations");

            var identity = new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.Role, user.Role)
            },CookieAuthenticationDefaults.AuthenticationScheme);

            var principal = new ClaimsPrincipal(identity);

            await HttpContext.SignInAsync(principal);

            return Redirect(returnUrl);
        }

        public IActionResult Register() => View();

        [HttpPost]
        public IActionResult Register(User model)
        {
            if (!ModelState.IsValid)
                return BadRequest("invalide");

            db.Users.Add(model);
            db.SaveChanges();

            return RedirectToAction(nameof(Login));
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return Redirect("/");
        }

    }
}