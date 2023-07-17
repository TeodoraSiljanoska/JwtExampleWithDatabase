using Microsoft.AspNetCore.Mvc;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using JwtExampleWithDatabase.Models;
using JwtExampleWithDatabase.Data;

namespace JwtExampleWithDatabase.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserRepository _userRepository;
        private readonly AppSettings _appSettings;
       //private readonly DataContext _dataContext;

        public UserController(IUserRepository userRepository, IOptions<AppSettings> appSettings)
        {
            _userRepository = userRepository;
            _appSettings = appSettings.Value;
        }

        [AllowAnonymous]
        [HttpPost("register")]
        public async Task<IActionResult> Register(string username, string password)
        {
            var existingUser = await _userRepository.GetByUsernameAsync(username);
            if (existingUser != null)
                return BadRequest("Username already exists");
            //var lastUser =  _dataContext.Users.OrderByDescending(a => a.Id).First();
            var user = new User
            {
                
               // Id = lastUser.Id+1,
                Username = username,
                Password = password,
                Role = "User"
            };

            await _userRepository.AddAsync(user);
            await _userRepository.SaveChangesAsync();

            return Ok("Registration successful");
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<IActionResult> Login(string username, string password)
        {
            var user = await _userRepository.ValidateLogin(username,password);
            if (user == null)
                return Unauthorized("Invalid username or password");
            else
            {
                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(new[]
                    {
                    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                    new Claim(ClaimTypes.Name, user.Username),
                    new Claim(ClaimTypes.Role, await _userRepository.GetUserRoleAsync(user.Username))
                }),
                    Expires = DateTime.UtcNow.AddDays(1),
                    SigningCredentials = new SigningCredentials(
                        new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_appSettings.Secret)),
                        SecurityAlgorithms.HmacSha256Signature)
                };

                var tokenHandler = new JwtSecurityTokenHandler();
                var token = tokenHandler.CreateToken(tokenDescriptor);
                var tokenString = tokenHandler.WriteToken(token);

                return Ok(new { Token = tokenString });
            }
        }

        [HttpGet("test")]
        [Authorize(Roles = "Admin")]
        public IActionResult TestAdminOnly()
        {
            return Ok("This endpoint is accessible only by users with the 'Admin' role.");
        }
    }
}
