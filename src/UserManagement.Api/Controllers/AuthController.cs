using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using UserManagement.Application.Interfaces; // Asegúrate de tener la interfaz de servicio adecuada
using UserManagement.Domain.Entities; // Asegúrate de tener un modelo para las credenciales de usuario

namespace UserManagement.Api.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IUserService _userService; // Servicio para manejar la lógica de usuario
        private readonly string _jwtSecret; // La clave secreta para firmar el JWT
         private readonly string _jwtSubject;
        private readonly string _jwtIssuer;
        private readonly string _jwtAudience;
        public AuthController(IUserService userService, IConfiguration configuration)
        {
            _userService = userService;
            _jwtSecret = configuration["Jwt:Key"]; // Asegúrate de que esté configurado en tu appsettings.json
            _jwtSubject = configuration["Jwt:Subject"];
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestModel model)
        {
            var user = await _userService.AuthenticateAsync(model.Username, model.Password); // Implementa este método en tu servicio

            if (user == null)
                return Unauthorized();

            var token = GenerateJwtToken(user);

            return Ok(new { Token = token });
        }

        private string GenerateJwtToken(User user)
        {


            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub,_jwtSubject ),
                new Claim(JwtRegisteredClaimNames.Jti,Guid.NewGuid().ToString() ),
                new Claim(JwtRegisteredClaimNames.Iat,DateTime.UtcNow.ToString() ),
                new Claim("id",user.Id.ToString()),

            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSecret));
            var sigiIn = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);


            var token = new JwtSecurityToken(
                _jwtIssuer,
                 _jwtAudience,
                 claims,
                 expires: DateTime.Now.AddMinutes(60),
                 signingCredentials:sigiIn

                );


            return new JwtSecurityTokenHandler().WriteToken(token);

        }
    }
}
