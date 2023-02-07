using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using EmployeeWEB.Models;
using EmployeeWEB.Utility;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeWEB.Controllers
{
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
                return View(await util.GetAllAsync(Resource.EmployeeAPIUrl));
            }catch (Exception ex)
            {
                ModelState.AddModelError("", "Error al obtener los empleados: " + ex.Message);
                return View();
            }
        }
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
                var modelStateError = await util.CreateAsync(Resource.EmployeeAPIUrl, employee);
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
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();
            var employee = await util.GetAsync(Resource.EmployeeAPIUrl, id.GetValueOrDefault());
            if (employee == null) return NotFound();

            return View(employee);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Employee employee)
        {
            if (ModelState.IsValid)
            {
                var modelStateError = await util.UpdateAsync(Resource.EmployeeAPIUrl + employee.Id, employee);
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
            var employee = await util.GetAsync(Resource.EmployeeAPIUrl, id.GetValueOrDefault());
            if (employee == null) return NotFound();
            return View(employee);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {  
                var employee = await util.GetAsync(Resource.EmployeeAPIUrl, id);

                if (employee == null) return NotFound();
                var modelStateError = await util.DeleteAsync(Resource.EmployeeAPIUrl, id);

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