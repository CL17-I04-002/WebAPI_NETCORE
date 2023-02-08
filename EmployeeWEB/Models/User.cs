using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeeWEB.Models
{
    public class User
    {
        public User()
        {
            Errors = new List<Errors>();
        }

        [Required(ErrorMessage = "El email es requerido")]
        public string Email { get; set; }
        [Required(ErrorMessage = "La contrasenia es requerida")]
        public string Password { get; set; }
        public List<Errors> Errors { get; set; }
    }
}
