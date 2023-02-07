using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using EmployeeAPI.Model.DTO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace EmployeeAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly UserManager<IdentityUser> userManager;
        private readonly SignInManager<IdentityUser> signInManager;
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly IConfiguration configuration;

        public UserController(UserManager<IdentityUser> userManager,
            SignInManager<IdentityUser> signInManager, RoleManager<IdentityRole> roleManager, IConfiguration configuration)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.roleManager = roleManager;
            this.configuration = configuration;
        }

        [HttpPost("Register")]
        public async Task<IActionResult> Register([FromBody] UserDTO userDTO)
        {
            var user = new IdentityUser { UserName = userDTO.Email, Email = userDTO.Email };

            IdentityResult result = await userManager.CreateAsync(user, userDTO.Password);

            if (result.Succeeded)
            {
                if(!await roleManager.RoleExistsAsync("Admin"))
                {
                    await roleManager.CreateAsync(new IdentityRole("Admin"));
                }
                if (!await roleManager.RoleExistsAsync("Customer"))
                {
                    await roleManager.CreateAsync(new IdentityRole("Customer"));
                }
                await userManager.AddToRoleAsync(user, "Customer");

                return NoContent();
            }
            foreach (var item in result.Errors)
            {
                ModelState.AddModelError("Response", item.Description);
            }
            return StatusCode(400, ModelState);
        }
        [HttpPost("Login")]
        public async Task<ActionResult<UserTokenDTO>> Login([FromBody] UserDTO userDTO)
        {
            ////Vamos a usar un metodo que nos permita iniciar sesion
            var result = await signInManager.PasswordSignInAsync(userDTO.Email, userDTO.Password, isPersistent:false, lockoutOnFailure:false);

            if (result.Succeeded)
            {
                var user = await userManager.FindByEmailAsync(userDTO.Email);
                var roles = await userManager.GetRolesAsync(user);
                var token = CreateToken(user, roles);

                return new UserTokenDTO
                {
                    Token = token,
                    UserName = user.UserName,
                    Roles = roles
                };
            }
            ModelState.AddModelError("Response", "Nombre de usuario/contrasenia no valido");

            return StatusCode(400, ModelState);
        }

        private string CreateToken(IdentityUser user, IList<string> roles)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWT:key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            /*Los claims son como contenedores de información del usuario para posterior transmitirlos 
             * y en base a eso poder autorizar hasta cierto punto al usuario*/

            /*Los claims son seguros y pueden ser firmados y encriptados para garantizar su integridad
             * y confidencialidad durante la transmisión. En resumen, los claims son una forma eficaz y escalable de transmitir información del usuario y autorizar el acceso a recursos en una aplicación.
             */
            var claims = new List<Claim>()
            {
                ///Vamos agregar un identificador unico
                new Claim(ClaimTypes.NameIdentifier, user.Id)
        };
            ///Vamos agregar la lista de roles
            foreach (var rolName in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, rolName));
            }
            ///A traves de este objeto vamos agregar informacion del token
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = creds
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}