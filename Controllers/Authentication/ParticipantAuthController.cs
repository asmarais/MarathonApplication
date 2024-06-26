﻿using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using MarathonApplication.Data;
using MarathonApplication.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;

namespace MarathonApplication.Controllers.Authentication
{
	[Route("api/auth/participant")]
	[ApiController]
	public class ParticipantAuthController : ControllerBase
	{
		private readonly ApplicationDbContext _db;
		private readonly IConfiguration _conf;

		public ParticipantAuthController(ApplicationDbContext db, IConfiguration conf)
		{
			_db = db;
			_conf = conf;
		}

		public class LoginDto
		{
			public string Email { get; set; }
			public string Password { get; set; }
		}

		public class ParticipantDto
		{
			public string FirstName { get; set; }
			public string SecondName { get; set; }
			public string Email { get; set; }
			public string Phone { get; set; }
			public string Password { get; set; }
		}

		[HttpPost("register")]
		public async Task<ActionResult<Participant>> Register(ParticipantDto request)
		{
			var result = await _db.Participants.FirstOrDefaultAsync(r => r.Email == request.Email);
			if (result != null)
			{
				return BadRequest("User exists");
			}

			string passwordHash = BCrypt.Net.BCrypt.HashPassword(request.Password);
			var participant = new Participant(
				request.FirstName,
				request.SecondName,
				request.Email,
				request.Phone,
				passwordHash
			);

			_db.Participants.Add(participant);
			await _db.SaveChangesAsync();

			return Ok(participant);
		}

		[HttpPost("login")]
		public async Task<ActionResult> Login(LoginDto request)
		{
			var participant = await _db.Participants.FirstOrDefaultAsync(u => u.Email == request.Email);
			if (participant == null || !BCrypt.Net.BCrypt.Verify(request.Password, participant.PasswordHash))
			{
				return NotFound("Your credentials are wrong");
			}

			string accessToken = CreateToken(participant);
			var refreshToken = GenerateRefreshToken();

			participant.RefreshToken = refreshToken.Token;
			participant.TokenExpiryDate = refreshToken.Expires;
			_db.Participants.Update(participant);
			await _db.SaveChangesAsync();

			return Ok(new { AccessToken = accessToken, RefreshToken = refreshToken });
		}

		private string CreateToken(Participant participant)
		{
			var claims = new List<Claim> {
				new Claim(ClaimTypes.Email, participant.Email),
			};

			var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_conf["JWT:Key"]));
			var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

			var token = new JwtSecurityToken(
				claims: claims,
				expires: DateTime.Now.AddDays(1),
				signingCredentials: creds
			);

			return new JwtSecurityTokenHandler().WriteToken(token);
		}

		[HttpPost("refresh")]
		public async Task<ActionResult> RefreshToken(RefreshToken refreshToken)
		{
			var participant = await ValidateRefreshToken(refreshToken);
			if (participant == null)
			{
				return Unauthorized("Invalid refresh token");
			}

			var newAccessToken = CreateToken(participant);
			return Ok(new { AccessToken = newAccessToken });
		}

		private async Task<Participant> ValidateRefreshToken(RefreshToken refreshToken)
		{
			return await _db.Participants
				.Where(u => u.RefreshToken == refreshToken.Token && u.TokenExpiryDate >= DateTime.UtcNow)
				.FirstOrDefaultAsync();
		}

		private RefreshToken GenerateRefreshToken()
		{
			return new RefreshToken
			{
				Token = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64)),
				Expires = DateTime.Now.AddDays(30)
			};
		}

		[HttpPut("updateAdditional")]
		public async Task<IActionResult> UpdateAdditional(string email, int age, string gender, int height, int weight, string tshirt)
		{
			var participant = await _db.Participants.FirstOrDefaultAsync(p => p.Email == email);

			if (participant == null)
			{
				return BadRequest("This email doesn't exist");
			}

			participant.Age = age;
			participant.Gender = gender;
			participant.Height = height;
			participant.Weight = weight;
			participant.TshirtSize = tshirt;

			_db.Participants.Update(participant);
			await _db.SaveChangesAsync();

			return Ok();
		}
	}
}
