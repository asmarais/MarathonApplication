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
	[Authorize]

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
		public IActionResult CreateParticipant([FromBody] Participant participant)
		{
			if (ModelState.IsValid)
			{
				_db.Participants.Add(participant);
				 _db.SaveChanges();
				return CreatedAtAction(nameof(GetParticipants), new { id = participant.Id });
			}
			else
			{
				return BadRequest(ModelState);
			}
		}
		
	}
}
