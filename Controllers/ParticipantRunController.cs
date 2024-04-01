using MarathonApplication.Data;
using MarathonApplication.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace MarathonApplication.Controllers
{
	[Route("api/ParticipantRuns")]
	[ApiController]
	public class ParticipantRun : ControllerBase
	{
		private readonly ApplicationDbContext _db;

		public ParticipantRun(ApplicationDbContext db)
		{
			_db = db;
		}

		[HttpGet]
		public ActionResult<IEnumerable<ParticipantRun>> GetParticipantRuns()
		{
			var runs = _db.Participantsruns.ToList();
			return Ok(runs);
		}
		[HttpGet("{id:int}")]
		public ActionResult<ParticipantRun> GetParticipantRuns(int id)
		{
			if (id == 0)
			{
				return BadRequest();
			}
			var run = _db.Participantsruns.FirstOrDefault(u => u.Id == id);
			if (run == null)
			{
				return NotFound();
			}
			return Ok(run);
		}
		
	}
}
