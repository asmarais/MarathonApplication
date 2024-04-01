using MarathonApplication.Data;
using MarathonApplication.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace MarathonApplication.Controllers
{
	[Route("api/Events")]
	[ApiController]
	[Authorize]
	public class EventController : ControllerBase
	{
		private readonly ApplicationDbContext _db;

		public EventController(ApplicationDbContext db)
		{
			_db = db;
		}

		[HttpGet]
		public ActionResult<IEnumerable<Event>> GetEvents()
		{
			var events = _db.Events.ToList();
			return Ok(events);
		}
		[HttpGet("{id:int}")]
		public ActionResult<Event> GetEvents(int id)
		{
			if (id == 0)
			{
				return BadRequest();
			}
			var even = _db.Events.FirstOrDefault(u => u.Id == id);
			if (even == null)
			{
				return NotFound();
			}
			return Ok(even);
		}
		[HttpPost]
		public IActionResult CreateEvent([FromBody] Event even)
		{
			if (ModelState.IsValid)
			{
				_db.Events.Add(even);
				_db.SaveChanges();
				return CreatedAtAction(nameof(GetEvents), new { id = even.Id });
			}
			else
			{
				return BadRequest(ModelState);
			}
		}
	}
}
