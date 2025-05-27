using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ModelTracKer.Data;
using ModelTracKer.Dto;
using ModelTracKer.Models;
using ModelTracKer.Services.Interfaces;

namespace ModelTracKer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IJwtTokenService _tokenService;
        private readonly TrackerDbContext _context;

        public AuthController(IUserService userService, IJwtTokenService tokenService, TrackerDbContext context)
        {
            _userService = userService;
            _tokenService = tokenService;
            _context = context;
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto dto)
        {
            var user = await _userService.AuthenticateAsync(dto.Username, dto.Password);
            if (user == null)
                return Unauthorized();

            var token = _tokenService.GenerateToken(user.Username, user.Role.Name);
            return Ok(new { Token = token });
        }

        // POST: api/auth/register
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterUserDto dto)
        {
            var existingUser = await _context.Users
                .FirstOrDefaultAsync(u => u.Username == dto.Username);
            if (existingUser != null)
                return BadRequest("Username already exists.");

            // Check if role with provided ID exists
            var role = await _context.Roles.FindAsync(dto.Role);
            if (role == null)
                return BadRequest("Invalid role ID.");

            
            // Add to UserRoles
            var newUser = new User
            {
                Username = dto.Username,
                Password = dto.Password, 
                RoleId = role.Id
            };

            _context.Users.Add(newUser);
            await _context.SaveChangesAsync();

            // Generate JWT token (assuming single role)
            var token = _tokenService.GenerateToken(newUser.Username, role.Name);

            return Ok(new { Token = token });
        }

    }
}
