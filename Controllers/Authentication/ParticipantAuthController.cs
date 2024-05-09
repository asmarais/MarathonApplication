using Azure.Core;
using MarathonApplication.Data;
using MarathonApplication.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

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

		[HttpPost("register")]
		public ActionResult<Participant> Register(ParticipantDto request)
		{
			var result = _db.Participants.FirstOrDefault(r => r.Email == request.Email);
			if (result != null)
			{
				return BadRequest("User exists");
			}
			string passwordHash
				= BCrypt.Net.BCrypt.HashPassword(request.Password);
			var participant = new Participant(request.FirstName,
											  request.SecondName,
												request.Birthday,
												request.Email,
												request.Phone,
												passwordHash);
			_db.Participants.Add(participant);
			_db.SaveChanges();

			return Ok(participant);
		}
		
		[HttpPost("login")]
		public ActionResult<Participant> Login(LoginDto request) 
		{		
			var participant = _db.Participants.FirstOrDefault(u => u.Email == request.Email);
			if (participant == null || !BCrypt.Net.BCrypt.Verify(request.Password, participant.PasswordHash))
			{
				return NotFound("Your credentials are wrong");
			}
			string accessToken = CreateToken(participant);
			var refreshToken = GenerateRefreshToken();

			participant.RefreshToken = refreshToken.Token;
			participant.TokenExpiryDate = refreshToken.Expires;
			_db.Participants.Update(participant);
			_db.SaveChanges();
			return Ok(new { AccessToken = accessToken, RefreshToken = refreshToken });
		}
		private string CreateToken(Participant participant)
		{
			List<Claim> claims = new List<Claim> {
				new Claim("Email", participant.Email),
				new Claim("Role", "mobile")
			};
			var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(
				_conf.GetSection("JWT:Key").Value!));
			var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

			var token = new JwtSecurityToken(
				claims: claims,
				expires: DateTime.Now.AddDays(1),
				signingCredentials: creds
				) ;
			var jwt = new JwtSecurityTokenHandler().WriteToken(token);
			return (jwt);
		}

		[HttpPost("refresh")]
		public async Task<ActionResult> RefreshToken(RefreshToken refreshToken)
		{
			var participant = ValidateRefreshToken(refreshToken);
			if (participant == null)
			{
				return Unauthorized("Invalid refresh token");
			}
			var newAccessToken = CreateToken(participant);
			return Ok(new
			{
				AccessToken = newAccessToken,
			});
		}

		private Participant ValidateRefreshToken(RefreshToken refreshToken)
		{
			var participant = _db.Participants.Where(u => u.RefreshToken == refreshToken.Token && u.TokenExpiryDate >= DateTime.UtcNow).FirstOrDefault();
			return participant;
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

