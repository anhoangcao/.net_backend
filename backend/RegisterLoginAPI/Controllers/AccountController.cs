using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using RegisterLoginAPI.Models;
using System.Threading.Tasks;
using BCrypt.Net;

namespace RegisterLoginAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public AccountController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser { UserName = model.Email, Email = model.Email, Name = model.Name };
                var result = await _userManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    return Ok(new
                    {
                        status = 200,
                        message = "Registration successful",
                        data = new
                        {
                            name = model.Name,
                            email = model.Email
                        }
                    });
                }
                return BadRequest(new { status = 400, message = "Registration failed", errors = result.Errors });
            }
            return BadRequest(new { status = 400, message = "Invalid data" });
        }

        private string HashPassword(string password)
        {
            // Sử dụng BCrypt để băm mật khẩu
            return BCrypt.Net.BCrypt.HashPassword(password);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, false, false);
                if (result.Succeeded)
                {
                    var hashedPassword = HashPassword(model.Password);
                    return Ok(new
                    {
                        status = 200,
                        message = "Login successful",
                        data = new
                        {
                            email = model.Email,
                            password = hashedPassword
                        }
                    });
                }
                return BadRequest(new { status = 400, message = "Invalid login attempt" });
            }
            return BadRequest(new { status = 400, message = "Invalid data" });
        }
    }
}
