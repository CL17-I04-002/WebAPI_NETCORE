using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using EmployeeAPI.Model;
using EmployeeAPI.Model.DTO;
using EmployeeAPI.Repository.IRepository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        private readonly IEmployeeRepository employeeRepository;
        private readonly IMapper mapper;

        public EmployeeController(IEmployeeRepository employeeRepository, IMapper mapper)
        {
            this.employeeRepository = employeeRepository;
            this.mapper = mapper;
        }
        [HttpGet]
        public IActionResult GetAllEmployees()
        {
            var employee = employeeRepository.GetAllEmployee();
            return Ok(mapper.Map<List<EmployeeDTO>>(employee));
        }

        [HttpGet("{id}")]
        ///Con ActionResult estamos evitando que sea generico y en este caso estamos diciendo que retornara un tipo ProductDTO
        public ActionResult<EmployeeDTO> GetEmployee([FromRoute] int id)
        {
            var employee = employeeRepository.GetEmployee(id);
            ///Código 404
            if (employee == null) return NotFound();
            ///Código 200
            return mapper.Map<EmployeeDTO>(employee);
        }
        [HttpPost]
        public ActionResult<Employee> CreateEmployee([FromBody] EmployeeDTO employeeDTO)
        {
            if (!ModelState.IsValid) return BadRequest();

            /*if (!employeeRepository.ExistEmployee(employeeDTO.Name))
            {
                return ModelState.AddModelError("Response", $"Ya existe un empleado ")
            }*/

            var employee = mapper.Map<Employee>(employeeDTO);

            if (!employeeRepository.SaveEmployee(employee))
            {
                ModelState.AddModelError("Response", $"Ocurrio un problema no se pudo registrar el nombre de empleado: {employeeDTO.Name}");
                return StatusCode(500, ModelState);
            }
            return employee;
        }
        [HttpPut("{id}")]
        public IActionResult UpdateEmployee(int id, [FromBody] EmployeeDTO employeeDTO)
        {
            if (id != employeeDTO.Id) return NotFound();

            var employee = mapper.Map<Employee>(employeeDTO);

            if (!employeeRepository.UpdateEmployee(employee))
            {
                ModelState.AddModelError("Response", $"Ocurrio un problema no se pudo actualizar el empleado: {employeeDTO.Name}");
                return StatusCode(500, ModelState);
            }
            return NoContent();
        }
        [HttpDelete("{id}")]
        public IActionResult DeleteEmployee(int id)
        {
            if (!employeeRepository.ExistEmployee(id)) return NotFound();

            var employee = employeeRepository.GetEmployee(id);

            if (!employeeRepository.DeleteEmployee(employee))
            {
                ModelState.AddModelError("Response", $"Ocurrio un problema no se pudo eliminar el empleado: {employee.Name}");
                return StatusCode(500, ModelState);
            }

            return NoContent();
        }
    }
}