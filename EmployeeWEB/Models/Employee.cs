using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeeWEB.Models
{
    public class Employee
    {
        public Employee()
        {
            Errors = new List<Errors>();
        }
        public int Id { get; set; }
        [Required(ErrorMessage = "El nombre de empleado es requerido")]
        public string Name { get; set; }
        public string LastName { get; set; }
        [Required]
        [RegularExpression(@"^\d+$", ErrorMessage = "El campo HoursWorked solo puede contener números")]
        public int HoursWorked { get; set; }
        [Required(ErrorMessage = "El campo Job es requerido")]
        public string Job { get; set; }
        [RegularExpression(@"^\d+(\.\d{1,2})?$", ErrorMessage = "El campo Salary solo puede contener números decimales")]
        public Double Salary { get; set; }
        public List<Errors> Errors { get; set; }
    }
}
