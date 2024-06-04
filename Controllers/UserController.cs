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

			_db.Entry(user).Reference(u => u.Role).Load();

			if (user.Role == null)
			{
				return NotFound("Role not found for the user.");
			}

			var result = new
			{
				UserName = user.Username,
				Role = user.Role.role,
			};

			return Ok(result);
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
		public class UserUpdateDto
		{
			public required string Username { get; set; }
			public required string Role { get; set; }
		}

		[HttpPut("{id:int}")]
		public IActionResult UpdateUser(int id, [FromBody] UserUpdateDto updatedUser)
		{
			if (updatedUser == null)
			{
				return BadRequest("User data is null");
			}

			var existingUser = _db.Users.Include(u => u.Role).FirstOrDefault(u => u.Id == id);
			if (existingUser == null)
			{
				return NotFound();
			}

			existingUser.Username = updatedUser.Username;

			if (updatedUser.Role != null)
			{
				var existingRole = _db.Roles.FirstOrDefault(r => r.role == updatedUser.Role);
				if (existingRole != null)
				{
					existingUser.Role = existingRole;
				}
				else
				{
					return BadRequest("Invalid role ID");
				}
			}

			_db.Users.Update(existingUser);
			_db.SaveChanges();

			return Ok(existingUser);
		}

	}
}
