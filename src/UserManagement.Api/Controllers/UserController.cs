using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using UserManagement.Application.Interfaces;
using UserManagement.Domain.Entities;

namespace UserManagement.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

    

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

       
        [HttpGet("Users")]
        public async Task<IActionResult> GetUsers()
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;

           


            var users = await _userService.GetUsersAsync();
            return Ok(users);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetUser(int id)
        {
            var user = await _userService.GetUserAsync(id);
            return user != null ? Ok(user) : NotFound();
        }

        [HttpPost]
        public async Task<IActionResult> AddUser([FromBody] User user)
        {   // Implementación de la acción
                await _userService.AddUserAsync(user);
            return CreatedAtAction(nameof(GetUser), new { id = user.Id }, user);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(int id, [FromBody] User user)
        {
            var existingUser = await _userService.GetUserAsync(id);
            if (existingUser == null) return NotFound();

            user.Id = id;
            // Implementación de la acción
               await _userService.UpdateUserAsync(user);
            return Ok(user);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var existingUser = await _userService.GetUserAsync(id);
            if (existingUser == null) return NotFound();

            await _userService.DeleteUserAsync(id);
            return NoContent();
        }
    }
}
