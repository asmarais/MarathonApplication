using MarathonApplication.Data;
using MarathonApplication.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace MarathonApplication.Controllers
{
	[Authorize(Roles = "Admin")]
	[Route("api/Users")]
	[ApiController]
	public class UserController : ControllerBase
	{
		private readonly ApplicationDbContext _db;

		public UserController(ApplicationDbContext db)
		{
			_db = db;
		}
		[HttpGet]
		public ActionResult<IEnumerable<object>> GetUsers()
		{
			var users = _db.Users.Include(u => u.Role).ToList();  
			var userInfos = new List<object>();

			foreach (var user in users)
			{
				var userInfo = new
				{
					id = user.Id,
					Username = user.Username,
					Role =  user.Role.role 
				};
				userInfos.Add(userInfo);
			}

			return Ok(userInfos);
		}


		[HttpGet("{id:int}")]
		public ActionResult<object> GetUsers(int id)
		{
			if (id == 0)
			{
				return BadRequest();
			}
			var user = _db.Users.FirstOrDefault(u => u.Id == id);
			if (user == null)
			{
				return NotFound();
			}
			//I want to have the first element not a list
			var role = _db.Roles.Where(u => u.Id == user.Role.Id);
			var results = new
			{
				UserName = user.Username,
				Role = role		
			};
			return Ok(results);
		}
		
		[HttpDelete("{id:int}")]
		public IActionResult DeleteUser(int id)
		{
			User? obj = _db.Users.Find(id);
			if (obj == null)
			{
				return NotFound();
			}
			
			_db.Users.Remove(obj);
			_db.SaveChanges();
			return NoContent();
		}
		
		[HttpPut("{id:int}")]
		public IActionResult UpdateUser(int id, [FromBody] User user)
		{
			if (user == null)
			{
				return NotFound();
			}
			_db.Users.Update(user);
			_db.SaveChanges();
			return Ok(user);
		}
		
	}
}
