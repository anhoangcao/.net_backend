using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using RegisterLoginAPI.Models;
using System.Data.SqlClient;
using System.Threading.Tasks;
using BCrypt.Net;
using Microsoft.Extensions.Configuration;

namespace RegisterLoginAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IConfiguration _configuration;

        public AccountController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, IConfiguration configuration)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _configuration = configuration;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterModel model)
        {
            if (ModelState.IsValid)
            {
                var hashedPassword = HashPassword(model.Password);
                var user = new ApplicationUser { UserName = model.Email, Email = model.Email, Name = model.Name, PasswordHash = hashedPassword };
                var result = await _userManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    // Lưu vào cơ sở dữ liệu
                    var connectionString = _configuration.GetConnectionString("DefaultConnection");
                    using (SqlConnection connection = new SqlConnection(connectionString))
                    {
                        connection.Open();
                        string query = "INSERT INTO Users (Name, Email, PasswordHash) VALUES (@Name, @Email, @PasswordHash)";
                        using (SqlCommand command = new SqlCommand(query, connection))
                        {
                            command.Parameters.AddWithValue("@Name", model.Name);
                            command.Parameters.AddWithValue("@Email", model.Email);
                            command.Parameters.AddWithValue("@PasswordHash", hashedPassword);
                            command.ExecuteNonQuery();
                        }
                    }

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
            return BCrypt.Net.BCrypt.HashPassword(password);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(model.Email);
                if (user == null)
                {
                    return BadRequest(new { status = 400, message = "Invalid login attempt" });
                }

                var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, false, false);
                if (result.Succeeded)
                {
                    return Ok(new
                    {
                        status = 200,
                        message = "Login successful",
                        data = new
                        {
                            name = user.Name,
                            email = model.Email
                        }
                    });
                }
                return BadRequest(new { status = 400, message = "Invalid login attempt" });
            }
            return BadRequest(new { status = 400, message = "Invalid data" });
        }
        [HttpPost("check-email")]
        public async Task<IActionResult> CheckEmail([FromBody] CheckEmailModel model)
        {
            if (string.IsNullOrEmpty(model.Email))
            {
                return BadRequest(new { status = 400, message = "Email is required" });
            }

            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user != null)
            {
                return Ok(new { exists = true });
            }

            return Ok(new { exists = false });
        }
    }
    public class CheckEmailModel
    {
        public string? Email { get; set; }
    }
}
