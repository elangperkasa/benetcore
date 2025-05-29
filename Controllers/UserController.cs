using BENETCORE.Data;
using BENETCORE.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;


namespace BENETCORE.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly Data.tDbContext _context;
        //private readonly IConfiguration _config;

        public UserController(Data.tDbContext context)
        {
            _context = context;
        }

        //[HttpPost("login")]
        //public async Task<IActionResult> Login([FromBody] LoginRequest request)
        //{
        //    var user = await _context.User
        //        .FirstOrDefaultAsync(u => u.Email == request.Email && u.Status == "Active");

        //    if (user == null || !BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash))
        //    {
        //        return Unauthorized("Invalid credentials");
        //    }

        //    var token = GenerateJwtToken(user);
        //    return Ok(new { token });
        //}

        //private string GenerateJwtToken(User user)
        //{
        //    var claims = new[]
        //    {
        //    new Claim(JwtRegisteredClaimNames.Sub, user.Id),
        //    new Claim(JwtRegisteredClaimNames.Email, user.Email),
        //    new Claim("name", user.Name),
        //    new Claim("status", user.Status)
        //};

        //    var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
        //    var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        //    var token = new JwtSecurityToken(
        //        issuer: _config["Jwt:Issuer"],
        //        claims: claims,
        //        expires: DateTime.Now.AddHours(2),
        //        signingCredentials: creds);

        //    return new JwtSecurityTokenHandler().WriteToken(token);
        //}

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var items = await _context.User.ToListAsync();
            return Ok(items);
        }

        [HttpPost]
        public async Task<IActionResult> CreateUser([FromBody] CreateUserRequest request)
        {
            //Validate email uniqueness
            if (await _context.User.AnyAsync(u => u.Email == request.Email))
                return BadRequest("Email already in use.");

            //Hash password
            var hashedPassword = BCrypt.Net.BCrypt.HashPassword(request.Password);

            var user = new Data.User
            {
                Id = Guid.NewGuid().ToString(),
                Name = request.Name,
                Email = request.Email,
                PhoneNumber = request.PhoneNumber,
                PasswordHash = hashedPassword,
                Status = request.Status,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = null
            };

            _context.User.Add(user);
            await _context.SaveChangesAsync();

            //return CreatedAtAction(nameof(GetUserById), new { id = user.Id }, user);
            return Ok(user);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetUserById(string id)
        {
            var user = await _context.User.FindAsync(id);
            if (user == null) return NotFound();
            return Ok(user);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(string id, [FromBody] UpdateUserRequest request)
        {
            var user = await _context.User.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            user.Name = request.Name;
            user.Email = request.Email;
            user.PhoneNumber = request.PhoneNumber;
            user.Status = request.Status;
            user.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            return Ok(user);
        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(string id)
        {
            var user = await _context.User.FindAsync(id);
            if (user == null)
            {
                return NotFound(new { message = $"User with ID {id} not found." });
            }

            _context.User.Remove(user);
            await _context.SaveChangesAsync();

            return Ok(new { message = $"User with ID {id} deleted successfully." });
        }


    }
}
