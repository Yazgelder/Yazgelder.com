using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Yazgelder.Entity;
using Yazgelder.Entity.Models;
using Yazgelder.Infrastructure;

namespace Yazgelder.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Route("/Admin/Login")]
    [Microsoft.AspNetCore.Authorization.Authorize]
    public class LoginController : Controller
    {
        private readonly Context _context;
        private readonly ISysFunctions _sysFunctions;

        public LoginController(Context context, ISysFunctions sysFunctions)
        {
            _context = context;
            _sysFunctions = sysFunctions;
        }

        [AllowAnonymous]
        [HttpGet]
        public IActionResult Login()
        {
            return View(new User());
        }

        [HttpPost]
        [AllowAnonymous]
        [ServiceFilter(typeof(ValidateReCaptchaAttribute))]
        [HttpPost]
        public IActionResult Login(User model)
        {
            if (ModelState.IsValid)
            {
                var md5 = _sysFunctions.GetMd5Hash(model.Password);
                if (!_context.User.Any())
                {
                    _context.User.Add(new Entity.Models.User() { Id = Guid.NewGuid(), UserName = model.UserName, Password = md5 });
                    _context.SaveChanges();
                }
                if (_context.User.Any(x => x.UserName == model.UserName && x.Password == md5))
                {
                    var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Name, model.UserName)
                    };
                    var userIdentity = new ClaimsIdentity(claims, "org.Yazgelder.Cookie");

                    ClaimsPrincipal principal = new ClaimsPrincipal(userIdentity);
                    Microsoft.AspNetCore.Authentication.AuthenticationHttpContextExtensions.SignInAsync(HttpContext, "org.Yazgelder.Cookie", principal);
                    return Redirect("/Admin/Home");
                }
                else
                {
                    ModelState.AddModelError("1", "Yanlış kullanıcı adı veya şifre.");
                }
            }
            else
            {
                ModelState.AddModelError("1", "Veriler Onaylanmadı. Lütfen sayfanızı yenileyip tekrar deneyiniz.");
            }
            return View(model);
        }

        [HttpPost("/Admin/SignOut")]
        public IActionResult SignOut()
        {
            Microsoft.AspNetCore.Authentication.AuthenticationHttpContextExtensions.SignOutAsync(HttpContext, "org.Yazgelder.Cookie");
            return Redirect("/");
        }
    }
}