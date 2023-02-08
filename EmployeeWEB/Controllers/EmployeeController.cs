using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using EmployeeWEB.Models;
using EmployeeWEB.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeWEB.Controllers
{
    [Authorize]
    public class EmployeeController : Controller
    {
        private readonly IHttpClientFactory httpClientFactory;
        private readonly Util<Employee> util;

        public EmployeeController(IHttpClientFactory httpClientFactory)
        {
            this.httpClientFactory = httpClientFactory;
            this.util = new Util<Employee>(httpClientFactory);

        }
        public async Task<IActionResult> Index()
        {
            try
            {
                return View(await util.GetAllAsync(Resource.EmployeeAPIUrl, HttpContext.Session.GetString("Token")));
            }catch (Exception ex)
            {
                ModelState.AddModelError("", "Error al obtener los empleados: " + ex.Message);
                return View();
            }
        }
        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            return View(new Employee());
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Employee employee)
        {
            if (ModelState.IsValid)
            {
                var modelStateError = await util.CreateAsync(Resource.EmployeeAPIUrl, employee, HttpContext.Session.GetString("Token"));
                if(modelStateError.Response.Errors.Count > 0)
                {
                    foreach(var item in modelStateError.Response.Errors)
                    {
                        employee.Errors.Add(item);
                    }
                    return View(employee);
                }
                return RedirectToAction(nameof(Index));
            }
            return View(employee);
        }
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();
            var employee = await util.GetAsync(Resource.EmployeeAPIUrl, id.GetValueOrDefault(), HttpContext.Session.GetString("Token"));
            if (employee == null) return NotFound();

            return View(employee);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Employee employee)
        {
            if (ModelState.IsValid)
            {
                var modelStateError = await util.UpdateAsync(Resource.EmployeeAPIUrl + employee.Id, employee, HttpContext.Session.GetString("Token"));
                if(modelStateError.Response.Errors.Count > 0)
                {
                    foreach(var item in modelStateError.Response.Errors)
                    {
                        employee.Errors.Add(item);
                    }
                    return View(employee);
                }
                return RedirectToAction(nameof(Index));
            }
            return View(employee);
        }
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();
            var employee = await util.GetAsync(Resource.EmployeeAPIUrl, id.GetValueOrDefault(), HttpContext.Session.GetString("Token"));
            if (employee == null) return NotFound();
            return View(employee);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {  
                var employee = await util.GetAsync(Resource.EmployeeAPIUrl, id, HttpContext.Session.GetString("Token"));

                if (employee == null) return NotFound();
                var modelStateError = await util.DeleteAsync(Resource.EmployeeAPIUrl, id, HttpContext.Session.GetString("Token"));

                if(modelStateError.Response.Errors.Count > 0)
                {
                    foreach(var item in modelStateError.Response.Errors)
                    {
                        employee.Errors.Add(item);
                    }
                    return View(employee);
                }
               return RedirectToAction(nameof(Index));
        } 
    }
}