using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Threading.Tasks;
using EmployeeWEB.Models;
using EmployeeWEB.Utility;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeWEB.Controllers
{
    public class AccountController : Controller
    {
        private readonly Util<User> util;
        public AccountController(IHttpClientFactory httpClientFactory)
        {
            util = new Util<User>(httpClientFactory);
        }
        public IActionResult Login()
        {
            return View(new User());
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(User user)
        {
            if (ModelState.IsValid)
            {
                var modelStateError = await util.LoginAsync(Resource.LoginAPIUrl, user);
                if (modelStateError.Response.Errors.Count > 0)
                {
                    foreach (var item in modelStateError.Response.Errors)
                    {
                        user.Errors.Add(item);
                    }
                    return View(user);
                }
                ///Si nuestro token es null
                if (modelStateError.Token == null) return View(user);

                ///Así vamos a obtener el esquema de autenticación basado en cookies
                var identity = new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationScheme);
                ///Vamos agregar información del usuario a este ClaimsIdentity
                identity.AddClaim(new Claim(ClaimTypes.Name, modelStateError.UserName));

                ///Vamos agregar la lista de roles
                foreach (var roleName in modelStateError.Roles)
                {
                    identity.AddClaim(new Claim(ClaimTypes.Role, roleName));
                }

                var principal = new ClaimsPrincipal(identity);

                ///Vamos usar el método SignInAsync porque estamos usando el esquema de autenticación basado en Cookies
                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

                ///Vamos agregar a la sesion el token y nombre de usuario
                ///Acá practicamente creamos nuestro key Token y UserName el cual se usara más adelante
                HttpContext.Session.SetString("Token", modelStateError.Token);
                HttpContext.Session.SetString("UserName", modelStateError.UserName);
                return RedirectToAction("Index", "Home");
            }
            return View(user);
        }

        public IActionResult Register()
        {
            return View(new User());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(User user)
        {
            if (ModelState.IsValid)
            {
                var modelStateError = await util.RegisterAsync(Resource.RegisterAPIUrl, user);
                if (modelStateError.Response.Errors.Count > 0)
                {
                    foreach (var item in modelStateError.Response.Errors)
                    {
                        user.Errors.Add(item);
                    }
                    return View(user);
                }
                return RedirectToAction(nameof(Login));
            }
            return View(user);
        }
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            HttpContext.Session.SetString("Token", string.Empty);
            return RedirectToAction("Index", "Home");
        }

        public IActionResult AccessDenied()
        {
            return View();
        }

    }
}