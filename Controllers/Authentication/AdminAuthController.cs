using MarathonApplication.Data;
using MarathonApplication.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace MarathonApplication.Controllers.Authentication
{
	[Route("api/auth")]
	[ApiController]
	public class AdminAuthController : ControllerBase
	{
		private readonly ApplicationDbContext _db;
		private readonly IConfiguration _conf;

		public AdminAuthController(ApplicationDbContext db, IConfiguration conf)
		{
			_db = db;
			_conf = conf;
		}

		[HttpPost("register")]
		public ActionResult<User> Register(UserDto request)
		{
			var result = _db.Users.FirstOrDefault(r => r.Username == request.Username);
			if (result != null)
			{
				return BadRequest("User exists");
			}
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
			var refreshToken = GenerateRefreshToken();
			user.RefreshToken = refreshToken.Token;
			user.TokenExpiryDate = refreshToken.Expires;
			_db.Users.Update(user);
			_db.SaveChanges();

			return Ok(new { AccessToken = accessToken, RefreshToken = refreshToken });
		}
		[HttpPost("refresh")]
		public async Task<ActionResult> RefreshToken(RefreshToken refreshToken)
		{
			var user = ValidateRefreshToken(refreshToken);
			if (user == null)
			{
				return Unauthorized("Invalid refresh token");
			}
			var newAccessToken = CreateAccessToken(user);

			return Ok(new 
			{
				AccessToken = newAccessToken,
			});
		}



		private User ValidateRefreshToken(RefreshToken refreshToken)
		{
			var user = _db.Users.Where(u => u.RefreshToken == refreshToken.Token && u.TokenExpiryDate >= DateTime.UtcNow).FirstOrDefault(); 
			return user;
		}
		private string CreateAccessToken(User user)
		{
			var role = GetUserRoleByUsername(user.Username);
			List<Claim> claims = new List<Claim> {
				new Claim("name", user.Username),
				new Claim("role", role.role)
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
		private RefreshToken GenerateRefreshToken()
		{
			var refreshToken = new RefreshToken
			{
				Token = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64)),
				Expires = DateTime.Now.AddDays(30)
		};
		return refreshToken;
		}


	}
}
