using Azure.Core;
using MarathonApplication.Data;
using MarathonApplication.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Reflection;
using System.Text.Json.Serialization;

namespace MarathonApplication.Controllers
{
	[Route("api/Participants")]
	[ApiController]

	public class ParticipantController : ControllerBase
	{
		private readonly ApplicationDbContext _db;

		public ParticipantController(ApplicationDbContext db)
		{
			_db = db;
		}
		
		[HttpGet]
		public ActionResult<IEnumerable<Participant>> GetParticipants()
		{
			var participants = _db.Participants.ToList();
			return Ok(participants);
		}
		[HttpGet("{id:int}")]
		public ActionResult<Participant> GetParticipants(int id)
		{
			if (id == 0)
			{
				return BadRequest();
			}
			var participant = _db.Participants.FirstOrDefault(u => u.Id == id);
			if (participant == null)
			{
				return NotFound();
			}
			return Ok(participant);
		}

		[HttpGet("ByEmail")]
		public ActionResult<Participant> GetParticipantByEmail([FromQuery] string email)
		{
			if (string.IsNullOrEmpty(email))
			{
				return BadRequest("Email parameter is required");
			}

			var participant = _db.Participants.FirstOrDefault(u => u.Email == email);

			if (participant == null)
			{
				return NotFound("Participant not found");
			}

			return Ok(participant);
		}


		[HttpPost]
		public IActionResult CreateParticipant([FromBody] Participant request)
		{
			if (!ModelState.IsValid)
			{
				return BadRequest(ModelState);
			}

			var existingParticipant = _db.Participants.FirstOrDefault(r => r.Email == request.Email);
			if (existingParticipant != null)
			{
				return BadRequest("User exists");
			}

			string passwordHash = BCrypt.Net.BCrypt.HashPassword(request.PasswordHash);

			var participant = new Participant(
				request.FirstName,
				request.LastName,
				request.Age,
				request.Height,
				request.Weight,
				request.TshirtSize,
				request.Email,
				request.Phone,
				passwordHash,
				request.Gender
			);

			try
			{
				_db.Participants.Add(participant);
				_db.SaveChanges();
				return CreatedAtAction(nameof(GetParticipants), new { id = participant.Id });
			}
			catch (Exception ex)
			{
				return StatusCode(500, "Internal server error");
			}
		}

		[HttpDelete("ByEmail")]
		public IActionResult DeleteParticipantByEmail(string email)
		{
			Participant? obj = _db.Participants.FirstOrDefault(p => p.Email == email);

			if (obj == null)
			{
				return NotFound();
			}
			_db.Participants.Remove(obj);
			_db.SaveChanges();
			return NoContent();
		}
		[HttpDelete("{id:int}")]
		public IActionResult DeleteParticipantById(int id)
		{
			Participant? obj = _db.Participants.FirstOrDefault(p => p.Id == id);

			if (obj == null)
			{
				return NotFound();
			}

			_db.Participants.Remove(obj);
			_db.SaveChanges();
			return NoContent();
		}

		public class UpdateParticipantDto
		{
			public string? Email { get; set; }
			public string FirstName { get; set; }
			public string SecondName { get; set; }
			public string Phone { get; set; }
			public int Age { get; set; }
			public int Height { get; set; }
			public int Weight { get; set; }
			public string TshirtSize { get; set; }
			public string Gender { get; set; }
		}
		[HttpPut("ByEmail")]
		public IActionResult UpdateParticipant(string email, UpdateParticipantDto requestt)
		{
			Participant? obj = _db.Participants.FirstOrDefault(p => p.Email == email);

			if (obj == null)
			{
				return NotFound();
			}
			obj.Phone = requestt.Phone;
			obj.FirstName = requestt.FirstName;
			obj.LastName = requestt.SecondName;
			obj.Phone = requestt.Phone;
			obj.Age = requestt.Age;
			obj.Height = requestt.Height;
			obj.Weight = requestt.Weight;
			obj.Gender = requestt.Gender;
			obj.TshirtSize = requestt.TshirtSize;

			_db.Participants.Update(obj);
			_db.SaveChanges();
			return Ok(obj);
		}
		[HttpPut("{id:int}")]
		public IActionResult UpdateParticipant(int id, [FromBody] UpdateParticipantDto request)
		{
			Participant? obj = _db.Participants.FirstOrDefault(p => p.Id == id);

			if (obj == null)
			{
				return NotFound();
			}
			obj.Email = request.Email;
			obj.FirstName = request.FirstName;
			obj.LastName = request.SecondName;
			obj.Phone = request.Phone;
			obj.Age = request.Age;
			obj.Height = request.Height;
			obj.Weight = request.Weight;
			obj.Gender = request.Gender;
			obj.TshirtSize = request.TshirtSize;

			_db.Participants.Update(obj);
			_db.SaveChanges();
			return Ok(obj);
		}

		[HttpPut("ChangePassword")]
		public IActionResult Change(string email, string password)
		{
			Participant? obj = _db.Participants.FirstOrDefault(p => p.Email == email);

			if (obj == null)
			{
				return NotFound();
			}
			obj.PasswordHash =  BCrypt.Net.BCrypt.HashPassword(password);
			_db.Participants.Update(obj);
			_db.SaveChanges();
			return Ok();
		}
	}
}
