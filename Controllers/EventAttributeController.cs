using MarathonApplication.Data;
using MarathonApplication.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace MarathonApplication.Controllers
{
	[Route("api/EventAttributes")]
	[ApiController]
	public class EventAttribute : ControllerBase
	{
		private readonly ApplicationDbContext _db;

		public EventAttribute(ApplicationDbContext db)
		{
			_db = db;
		}

		[HttpGet]
		public ActionResult<IEnumerable<EventAttribute>> GetEventAttribtes()
		{
			var attributes = _db.EventAttributes.ToList();
			return Ok(attributes);
		}
		[HttpGet("{id:int}")]
		public ActionResult<EventAttribute> GetEventAttribtes(int id)
		{
			if (id == 0)
			{
				return BadRequest();
			}
			var attribute = _db.EventAttributes.FirstOrDefault(u => u.Id == id);
			if (attribute == null)
			{
				return NotFound();
			}
			return Ok(attribute);
		}
		/*
		[HttpPost]
		public IActionResult CreateEventAttribute([FromBody] EventAttribute attribute)
		{
			if (ModelState.IsValid)
			{
				_db.EventAttributes.Add(attribute);
				_db.SaveChanges();
				return CreatedAtAction(nameof(GetEventAttribtes), new { id = attribute.Id });
			}
			else
			{
				return BadRequest(ModelState);
			}
		}
		*/
	}
}
