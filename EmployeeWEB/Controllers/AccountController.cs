using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using EmployeeWEB.Models;
using EmployeeWEB.Utility;
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
        public IActionResult Logout()
        {
            HttpContext.Session.SetString("Token", string.Empty);
            return RedirectToAction("Index", "Home");
        }

    }
}