using MarathonApplication.Data;
using MarathonApplication.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;

namespace MarathonApplication.Controllers.Authentication
{
	[Route("api/auth/admin")]
	[ApiController]
	public class AdminAuthController : ControllerBase
	{
		private readonly ApplicationDbContext _db;
		private readonly IConfiguration _conf;
		private readonly IHttpContextAccessor _httpContextAccessor;

		public AdminAuthController(ApplicationDbContext db, IConfiguration conf, IHttpContextAccessor httpContextAccessor)
		{
			_db = db;
			_conf = conf;
			_httpContextAccessor = httpContextAccessor;
		}

		[HttpPost("register")]
		public ActionResult<User> Register(UserDto request)
		{
			string passwordHash = BCrypt.Net.BCrypt.HashPassword(request.Password);
			Role role = _db.Roles.FirstOrDefault(r => r.role == request.Role);
			var user = new User(request.Username, passwordHash, role);

			_db.Users.Add(user);
			_db.SaveChanges();

			return Ok(user);
		}

		[HttpPost("login")]
		public ActionResult Login(UserDto request)
		{
			var user = _db.Users.FirstOrDefault(u => u.Username == request.Username);
			if (user == null || !BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash))
			{
				return NotFound("Your credentials are wrong");
			}

			string accessToken = CreateAccessToken(user);

			// Set access token as a cookie
			var cookieOptions = new CookieOptions
			{
				// Cookie is accessible only via HTTP (not JavaScript)
				HttpOnly = true,
				SameSite = SameSiteMode.None,
				// Set to true if using HTTPS
				Secure = false, 
				// Token expiration time
				Expires = DateTime.UtcNow.AddDays(1) 
			};

			//Response.Cookies.Append("AccessToken", accessToken, cookieOptions);
			HttpContext.Response.Cookies.Append("AccessToken", accessToken, cookieOptions);


			return Ok();
		}

		private string CreateAccessToken(User user)
		{
			var role = GetUserRoleByUsername(user.Username);
			List<Claim> claims = new List<Claim> {
				new Claim(ClaimTypes.Name, user.Username),
				new Claim(ClaimTypes.Role, role.role)
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
			return jwt;
		}

		private Role GetUserRoleByUsername(string username)
		{
			var user = _db.Users
				.Include(u => u.Role)
				.FirstOrDefault(u => u.Username == username);

			return user?.Role;
		}
	}
}
