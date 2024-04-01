using MarathonApplication.Data;
using MarathonApplication.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
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
		//Read data from claim I will change this using services to access data everywhere
		[HttpGet, Authorize]
		public ActionResult<object> Get()
		{
			var role = ClaimTypes.Role;
			return Ok(new { role});
		}
		
		[HttpPost("register")]
		public ActionResult<Participant> Register(ParticipantDto request)
		{ 
			string passwordHash
				= BCrypt.Net.BCrypt.HashPassword(request.Password);
			var participant = new Participant(request.FirstName,
											  request.SecondName,
												request.Country,
												request.ZipCode,
												request.City,
												request.Street,
												request.Birthday,
												request.BirthPlace,
												request.Gender,
												request.Email,
												request.Phone,
												request.NbMarathon,
												request.EmergencyPerson,
												request.EmergencyMobile,
												request.Remarks,
												passwordHash);
			_db.Participants.Add(participant);
			_db.SaveChanges();

			return Ok(participant);
		}
		
		[HttpPost("login")]
		public ActionResult<Participant> Login(ParticipantDto request) 
		{		
			var participant = _db.Participants.FirstOrDefault(u => u.Email == request.Email);
			if (participant == null 
				|| !BCrypt.Net.BCrypt.Verify(request.Password, participant.PasswordHash))
			{
				return NotFound("Your credentials are wrong");
			}
			string token = CreateToken(participant);
			return Ok(token);
		}
		private string CreateToken(Participant participant)
		{
			List<Claim> claims = new List<Claim> {
				new Claim(ClaimTypes.Email, participant.Email),
				new Claim(ClaimTypes.Role, "MobileUser"),
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
		
	}
}
