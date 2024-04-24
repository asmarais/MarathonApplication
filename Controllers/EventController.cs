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
		public ActionResult<object> GetEvents(int id)
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
			var attribute = _db.EventAttributes.ToList().Where(u => u.EventFK == id);
			var results = new
			{
				eventObj = even,
				attributeObj = attribute
			};
			return Ok(results);
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
		[HttpDelete("{id:int}")]
		public IActionResult DeleteEvent(int id)
		{
			Event? obj = _db.Events.Find(id);
			if (obj == null)
			{
				return NotFound();
			}
			var attributes = _db.EventAttributes.ToList().Where(u => u.EventFK == id);
			foreach(var attribute in attributes) {
				_db.EventAttributes.Remove(attribute);
			}
			_db.Events.Remove(obj);
			_db.SaveChanges();
			return NoContent();
		}
		/*
		[HttpPut("{id:int}")]
		public IActionResult UpdateEvent(int id, [FromBody] Event Event)
		{
			if (Event == null)
			{
				return NotFound();
			}
			_db.Events.Update(Event);
			_db.SaveChanges();
			return Ok(Event);
		}
		*/
	}
}
