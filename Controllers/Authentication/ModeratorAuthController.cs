using MarathonApplication.Data;
using MarathonApplication.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace MarathonApplication.Controllers.Authentication
{
	/*
	 
	[Route("api/auth/moderator")]
	[ApiController]
	public class ModeratorAuthController : ControllerBase
	{
		private readonly ApplicationDbContext _db;
		private readonly IConfiguration _conf;

		public ModeratorAuthController(ApplicationDbContext db, IConfiguration conf)
		{
			_db = db;
			_conf = conf;
		}
		[HttpPost("register")]
		public ActionResult<User> Register(UserDto request)
		{
			string passwordHash
				= BCrypt.Net.BCrypt.HashPassword(request.Password);
			var user = new User(request.Username, passwordHash);

			_db.Users.Add(user);
			_db.SaveChanges();

			return Ok(user);
		}

		[HttpPost("login")]
		public ActionResult<User> Login(UserDto request)
		{
			var user = _db.Users.FirstOrDefault(u => u.Username == request.Username);
			if (user == null
				|| !BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash))
			{
				return NotFound("Your credentials are wrong");
			}
			string token = CreateToken(user);

			return Ok(token);
		}
		private string CreateToken(User user)
		{
			List<Claim> claims = new List<Claim> {
				new Claim(ClaimTypes.Name, user.Username),
				new Claim(ClaimTypes.Role, "Moderator"),
			};
			var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(
				_conf.GetSection("JWT:Key").Value!));
			var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

			var token = new JwtSecurityToken(
				claims: claims,
				expires: DateTime.Now.AddDays(1),
				signingCredentials: creds
				);
			var jwt = new JwtSecurityTokenHandler().WriteToken(token);
			return (jwt);
		}
	}
	*/
}


