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
	public class EventAttributeController : ControllerBase
	{
		private readonly ApplicationDbContext _db;

		public EventAttributeController(ApplicationDbContext db)
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
		public ActionResult<EventAttribute> GetEventAttributes(int id)
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

		[HttpPost]
		public IActionResult CreateEventAttributes([FromBody] EventAttribute attribute)
		{
			try
			{
				if (ModelState.IsValid)
				{
					_db.EventAttributes.Add(attribute);
					_db.SaveChanges();
					return CreatedAtAction(nameof(GetEventAttributes), new { id = attribute.Id }, attribute.Id);
				}
				else
				{
					return BadRequest(ModelState);
				}
			}
			catch (Exception ex)
			{
				// Log the exception or handle it accordingly
				return StatusCode(500, "An error occurred while processing the request.");
			}
		}

		[HttpPut("{id}")]
		public IActionResult UpdateEventAttribute(int id, [FromBody] EventAttribute attribute)
		{
			if (id != attribute.Id)
			{
				return BadRequest();
			}
			var existingAttribute = _db.EventAttributes.Find(id);
			if (existingAttribute == null)
			{
				return NotFound();
			}
			existingAttribute.EventTypeFK = attribute.EventTypeFK;
			existingAttribute.EventFK = attribute.EventFK;

			_db.Entry(existingAttribute).State = EntityState.Modified;

			try
			{
				_db.SaveChanges();
				return Ok(existingAttribute);
			}
			catch (DbUpdateConcurrencyException)
			{
				if (!EventAttributeExists(id))
				{
					return NotFound();
				}
				else
				{
					throw;
				}
			}
		}
		private bool EventAttributeExists(int id)
		{
			return _db.EventAttributes.Any(e => e.Id == id);
		}


	}
}

