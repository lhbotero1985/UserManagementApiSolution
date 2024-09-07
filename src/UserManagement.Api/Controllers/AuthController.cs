using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using UserManagement.Application.Interfaces; // Asegúrate de tener la interfaz de servicio adecuada
using UserManagement.Domain.Entities;

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
            _jwtIssuer = configuration["Jwt:Key"];
            _jwtAudience = configuration["Jwt:Key"]; 
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
            // Generate JWT token


            //crear la informacion del usuario para token
            var userClaims = new[]
            {
                new Claim("IdUsuario",user.Id.ToString())
            };

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSecret!));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature);

            //crear detalle del token
            var jwtConfig = new JwtSecurityToken(
                claims: userClaims,
                expires: DateTime.UtcNow.AddMinutes(10),
                signingCredentials: credentials
                );

            return new JwtSecurityTokenHandler().WriteToken(jwtConfig);

        }
    }
}
