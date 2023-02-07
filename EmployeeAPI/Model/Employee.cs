using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeeAPI.Model
{
    public class Employee
    {
        public Employee()
        {
            EntryTime = DateTime.Now;
        }
        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        public string LastName { get; set; }
        [Required]
        public int HoursWorked { get; set; }
        public DateTime EntryTime { get; set; }
        [Required]
        public string Job { get; set; }
        public Double Salary { get; set; }
    }
}
