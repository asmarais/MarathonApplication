using MarathonApplication.Data;
using MarathonApplication.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace MarathonApplication.Controllers
{
	[Route("api/EventTypes")]
	[ApiController]
	public class EventTypeController : ControllerBase
	{
		private readonly ApplicationDbContext _db;

		public EventTypeController(ApplicationDbContext db)
		{
			_db = db;
		}

		[HttpGet]
		public ActionResult<IEnumerable<EventType>> GetTypes()
		{
			var types = _db.EventTypes.ToList();
			return Ok(types);
		}
		[HttpGet("{id:int}")]
		public ActionResult<EventType> GetTypes(int id)
		{
			if (id == 0)
			{
				return BadRequest();
			}
			var type = _db.EventTypes.FirstOrDefault(u => u.Id == id);
			if (type == null)
			{
				return NotFound();
			}
			return Ok(type);
		}
		[HttpPost]
		public IActionResult CreateEventType([FromBody] EventType type)
		{
			if (ModelState.IsValid)
			{
				_db.EventTypes.Add(type);
				_db.SaveChanges();
				return CreatedAtAction(nameof(GetTypes), new { id = type.Id });
			}
			else
			{
				return BadRequest(ModelState);
			}
		}
		[HttpDelete("{id:int}")]
		public IActionResult DeleteType(int id)
		{
			EventType? obj = _db.EventTypes.Find(id);
			if (obj == null)
			{
				return NotFound();
			}
			_db.EventTypes.Remove(obj);
			_db.SaveChanges();
			return NoContent();
		}
		[HttpPut("{id:int}")]
		public IActionResult UpdateItem(int id, [FromBody] EventType type)
		{
			if (type == null)
			{
				return NotFound();
			}
			_db.EventTypes.Update(type);
			_db.SaveChanges();
			return Ok(type);
		}
	}
}
