using Azure.Core;
using MarathonApplication.Data;
using MarathonApplication.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

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

		[HttpPost]
		public IActionResult CreateParticipant([FromBody] Participant request)
		{
			if (ModelState.IsValid)
			{
				string passwordHash
				= BCrypt.Net.BCrypt.HashPassword(request.PasswordHash);
				var participant = new Participant(request.FirstName,
												  request.LastName,
													request.Birthday,
													request.Email,
													request.Phone,
													passwordHash);
				_db.Participants.Add(participant);
				_db.SaveChanges();
				return CreatedAtAction(nameof(GetParticipants), new { id = participant.Id });
			}
			else
			{
				return BadRequest(ModelState);
			}
		}

		[HttpDelete("{id:int}")]
		public IActionResult DeleteParticipant(int id)
		{
			Participant? obj = _db.Participants.Find(id);
			if (obj == null)
			{
				return NotFound();
			}
			_db.Participants.Remove(obj);
			_db.SaveChanges();
			return NoContent();
		}
		[HttpPut("{id:int}")]
		public IActionResult UpdateParticipant(int id, [FromBody] Participant Participant)
		{
			if (Participant == null)
			{
				return NotFound();
			}
			_db.Participants.Update(Participant);
			_db.SaveChanges();
			return Ok(Participant);
		}
	}
}
