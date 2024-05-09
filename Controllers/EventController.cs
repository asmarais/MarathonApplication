using MarathonApplication.Data;
using MarathonApplication.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
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
		public ActionResult<IEnumerable<Event>> GetEvents()
		{
			var events = _db.Events.ToList();
			foreach (var obj in events)
			{
				if (obj.Status == "open" && obj.Start < DateTime.Today)
				{
					obj.Status = "closed";
					_db.Events.Update(obj);
					_db.SaveChanges();
				}
			}
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
		public IActionResult CreateEvent([FromForm] Event even)
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
					imageFile.CopyToAsync(fileStream);
				}

				_db.Events.Add(even);
				_db.SaveChanges();
				return CreatedAtAction(nameof(GetEvents), new { id = even.Id });

			}
			catch (Exception){
				return StatusCode(500);
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
			foreach (var attribute in attributes)
			{
				_db.EventAttributes.Remove(attribute);
			}
			_db.Events.Remove(obj);
			_db.SaveChanges();
			return NoContent();
		}

		
	}
}

