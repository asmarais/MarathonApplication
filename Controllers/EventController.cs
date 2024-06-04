using MarathonApplication.Data;
using MarathonApplication.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Reflection.Metadata.Ecma335;

namespace MarathonApplication.Controllers
{
	[Authorize]
	[Route("api/Events")]
	[ApiController]
	public class EventController : ControllerBase
	{
		private readonly ApplicationDbContext _db;
		private readonly IWebHostEnvironment _hostEnvironment;

		public EventController(ApplicationDbContext db, IWebHostEnvironment hostEnvironment)
		{
			_db = db;
			this._hostEnvironment = hostEnvironment;
		}

		[HttpGet]
		public async Task<ActionResult<IEnumerable<Event>>> GetEvents()
		{
			var events = _db.Events.ToList();
			foreach (var obj in events)
			{
				if (obj.Status == "open" && obj.Start < DateTime.Now)
				{
					obj.Status = "closed";
					_db.Events.Update(obj);
				}
			}
			await _db.SaveChangesAsync();

			events = _db.Events.ToList();

			var results = events.Select(even => new Event(
				even.Id,
				even.EventName,
				even.Description,
				even.Start,
				even.End,
				even.Status,
				even.ImageName,
				$"{Request.Scheme}://{Request.Host}{Request.PathBase}/Images/{even.ImageName}"
			));

			return Ok(results);
		}

		[HttpGet("{id:int}")]
		public async Task<IActionResult> GetEvent(int id)
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
			string imageUrl = String.Format("{0}://{1}{2}/Images/{3}", Request.Scheme, Request.Host, Request.PathBase, even.ImageName);

			var e = new Event(
				even.Id,
				even.EventName,
				even.Description,
				even.Start,
				even.End,
				even.Status,
				even.ImageName,
				imageUrl
			);
			var attribute = _db.EventAttributes
								 .Where(u => u.EventFK == id)
								 .Join(_db.EventTypes,
									   attr => attr.EventTypeFK,
									   type => type.Id,
									   (attr, type) => new
									   {
										   attr.Id,
										   attr.EventFK,
										   Type = type.Type
									   })
								 .ToList();
			var results = new
			{
				eventObj = even,
				attributeObj = attribute
			};
			return Ok(results);
		}

		[HttpPost]
		public async Task<IActionResult> CreateEvent([FromForm] Event even)
		{
			var imageFile = even.ImageFile;
			try
			{
				string imageName = new String(Path.GetFileNameWithoutExtension(imageFile.FileName).Take(10).ToArray()).Replace(' ', '-');
				imageName = imageName + DateTime.Now.ToString("yymmssfff") + Path.GetExtension(imageFile.FileName);
				var imagePath = Path.Combine(_hostEnvironment.ContentRootPath, "Images", imageName);
				even.ImageName = imageName;
				using (var fileStream = new FileStream(imagePath, FileMode.Create))
				{
					await imageFile.CopyToAsync(fileStream);
				}

				_db.Events.Add(even);
				_db.SaveChanges();
				return CreatedAtAction(nameof(GetEvent), new { id = even.Id });

			}
			catch (Exception)
			{
				return StatusCode(500);
			}
		}

		[HttpPut("{id:int}")]
		public async Task<IActionResult> UpdateEvent(int id, [FromForm] Event updatedEvent)
		{
			if (id != updatedEvent.Id)
			{
				return BadRequest();
			}

			var existingEvent = await _db.Events.FindAsync(id);
			if (existingEvent == null)
			{
				return NotFound();
			}

			existingEvent.EventName = updatedEvent.EventName;
			existingEvent.Description = updatedEvent.Description;
			existingEvent.Start = updatedEvent.Start;
			existingEvent.End = updatedEvent.End;
			existingEvent.Status = updatedEvent.Status;

			// Handle image update if a new image is uploaded
			if (updatedEvent.ImageFile != null)
			{
				string imageName = new String(Path.GetFileNameWithoutExtension(updatedEvent.ImageFile.FileName).Take(10).ToArray()).Replace(' ', '-');
				imageName = imageName + DateTime.Now.ToString("yymmssfff") + Path.GetExtension(updatedEvent.ImageFile.FileName);
				var imagePath = Path.Combine(_hostEnvironment.ContentRootPath, "Images", imageName);
				using (var fileStream = new FileStream(imagePath, FileMode.Create))
				{
					await updatedEvent.ImageFile.CopyToAsync(fileStream);
				}
				existingEvent.ImageName = imageName;
			}

			_db.Events.Update(existingEvent);
			await _db.SaveChangesAsync();

			return NoContent();
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
			foreach (var attribute in attributes)
			{
				_db.EventAttributes.Remove(attribute);
			}
			_db.Events.Remove(obj);
			_db.SaveChanges();
			return NoContent();
		}

		[HttpGet("GetFirstFiveOpenEvents")]
		public async Task<ActionResult<IEnumerable<Event>>> GetFirstFiveOpenEvents()
		{
			var openEvents = await _db.Events
				.Where(e => e.Status == "open")
				.OrderBy(e => e.Start)
				.Take(5)
				.Select(e => new Event(
					e.Id,
					e.EventName,
					e.Description,
					e.Start,
					e.End,
					e.Status,
					e.ImageName,
					$"{Request.Scheme}://{Request.Host}{Request.PathBase}/Images/{e.ImageName}"
				))
				.ToListAsync();

			return Ok(openEvents);
		}
	}
}
