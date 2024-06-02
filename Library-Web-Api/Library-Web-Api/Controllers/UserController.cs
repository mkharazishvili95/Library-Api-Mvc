using FluentValidation;
using Library_Web_Api.Database;
using Library_Web_Api.Helpers;
using Library_Web_Api.Identity;
using Library_Web_Api.Models;
using Library_Web_Api.Service;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Library_Web_Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly LibraryContext _context;
        private readonly AppSettings _appSettings;
        private readonly IValidator<User> _userValidator;
        public UserController(IUserService userService, LibraryContext context, AppSettings appSettings, IValidator<User> userValidator)
        {
            _userService = userService;
            _context = context;
            _appSettings = appSettings;
            _userValidator = userValidator;
        }

        [HttpPost("Register")]
        public async Task<IActionResult> Register([FromBody] User newUser)
        {
            var validatorResults = _userValidator.Validate(newUser);
            if (!validatorResults.IsValid)
            {
                return BadRequest(validatorResults.Errors);
            }

            await _userService.RegisterNewUser(newUser);
            await _context.SaveChangesAsync();
            return Ok(new { SuccessMessage = $"User: {newUser.UserName.ToUpper()} has been successfully registered." });
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody] UserLoginForm login)
        {
            var userLogin = await _userService.UserLoginForm(login);
            if (userLogin == null)
            {
                return BadRequest(new { message = "UserName or Password is incorrect." });
            }

            Loggs newLog = new Loggs
            {
                UserName = userLogin.UserName,
                UserRole = userLogin.Role.ToString(),
                UserId = userLogin.Id,
                LoggDate = DateTime.Now
            };
            await _context.Loggs.AddAsync(newLog);
            await _context.SaveChangesAsync();

            var tokenString = GenerateToken(userLogin);

            return Ok(new
            {
                Message = "You have successfully Logged.",
                UserName = userLogin.UserName,
                Role = userLogin.Role,
                Token = tokenString
            });
        }

        private string GenerateToken(User user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, user.Id.ToString()),
                    new Claim(ClaimTypes.Role, user.UserName)
                }),
                Expires = DateTime.UtcNow.AddDays(365),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}
