using MarathonApplication.Data;
using MarathonApplication.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;

namespace MarathonApplication.Controllers
{
	[Route("api/ParticipantRuns")]
	[ApiController]
	public class ParticipantRunController : ControllerBase
	{
		private readonly ApplicationDbContext _db;

		public ParticipantRunController(ApplicationDbContext db)
		{
			_db = db;
		}

		public class ParticipantrunDto
		{
			public DateTime Registration { get; set; } = DateTime.Now;
			public string Email { get; set; }
			public int EventAttributeID { get; set; }
		}

		[HttpPost]
		public IActionResult CreateRun([FromBody] ParticipantrunDto request)
		{
			if (ModelState.IsValid)
			{
				var Id = _db.Participants
				.Where(p => p.Email == request.Email)
				.Select(p => p.Id)
				.FirstOrDefault();

				if (Id == null)
				{
					return BadRequest("Participant not found.");
				}
				var participantRun = new Participantsrun
				{
					ParticipantFK = Id,
					EventAttributeFK = request.EventAttributeID,
					Status = "registered"
				};

				_db.Participantsruns.Add(participantRun);
				_db.SaveChanges();
				return Ok();
			}
			else
			{
				return BadRequest(ModelState);
			}
		}
		[HttpGet]
		public ActionResult<IEnumerable<object>> GetRuns()
		{
			var currentDate = DateTime.Now.Date;

			var runsToUpdate = _db.Participantsruns
				.Include(pr => pr.EventAttribute)
				.ThenInclude(ea => ea.Event)
				.Where(pr => pr.Status == "registered" && pr.EventAttribute.Event.Start.Date <= currentDate)
				.ToList();

			foreach (var run in runsToUpdate)
			{
				run.Status = "canceled";
			}

			_db.SaveChanges();

			var participantRuns = _db.Participantsruns
				.Include(pr => pr.EventAttribute)
				.ThenInclude(ea => ea.Event)
				.Include(pr => pr.EventAttribute.EventType)
				.Select(pr => new
				{
					Email = pr.Participant.Email,
					EventName = pr.EventAttribute.Event.EventName,
					EventType = pr.EventAttribute.EventType.Type,
					StartDate = pr.EventAttribute.Event.Start.Date,
					StartTime = pr.EventAttribute.Event.Start.TimeOfDay,
					ImageSrc = $"{Request.Scheme}://{Request.Host}{Request.PathBase}/Images/{pr.EventAttribute.Event.ImageName}",
					Status = pr.Status
				})
				.OrderBy(pr => pr.StartDate)
				.ToList();

			return Ok(participantRuns);

		}

		[HttpGet("GetRunsByIDParticipant")]
		public IActionResult GetRunsByIDParticipant(string email)
		{
			if (string.IsNullOrEmpty(email))
			{
				return BadRequest("Email cannot be empty.");
			}
			// Find the participant based on the email
			var participant = _db.Participants.FirstOrDefault(p => p.Email == email);

			if (participant == null)
			{
				return NotFound("Participant not found.");
			}

			// Get all runs associated with the participant
			var eventAttributeFKs = _db.Participantsruns
		.Where(pr => pr.ParticipantFK == participant.Id)
		.Select(pr => pr.EventAttributeFK)
		.ToList();


			return Ok(eventAttributeFKs);
		}
		[HttpGet("GetRunsByParticipant")]
		public IActionResult GetRunsByParticipant(string email)
		{
			if (string.IsNullOrEmpty(email))
			{
				return BadRequest("Email cannot be empty.");
			}

			var participant = _db.Participants.FirstOrDefault(p => p.Email == email);

			if (participant == null)
			{
				return NotFound("Participant not found.");
			}

			var participantId = participant.Id;

			var events = _db.Events.ToList();
			foreach (var obj in events)
			{
				if (obj.Status == "open" && obj.Start < DateTime.Now)
				{
					obj.Status = "closed";
					_db.Events.Update(obj);
				}
			}

			// Update participant runs status to canceled if their status is "registered"
			var eventAttributesToUpdate = _db.Participantsruns
				.Where(pr => pr.ParticipantFK == participantId && pr.EventAttribute.Event.Status.ToLower() == "closed")
				.ToList();

			foreach (var eventAttrToUpdate in eventAttributesToUpdate)
			{
				if (eventAttrToUpdate.Status != "completed")
				{
					eventAttrToUpdate.Status = "canceled";

				}
				_db.Participantsruns.Update(eventAttrToUpdate);
			}

			_db.SaveChanges();

			// Fetch updated eventAttributes
			var eventAttributes = _db.Participantsruns
		.Where(pr => pr.ParticipantFK == participantId && pr.Status.ToLower() == "registered")
				.Select(pr => new
				{
					EventAttributeFK = pr.EventAttributeFK,
					EventName = pr.EventAttribute.Event.EventName,
					EventType = pr.EventAttribute.EventType.Type,
					StartDate = pr.EventAttribute.Event.Start.Date,
					StartTime = pr.EventAttribute.Event.Start.TimeOfDay,
					ImageSrc = $"{Request.Scheme}://{Request.Host}{Request.PathBase}/Images/{pr.EventAttribute.Event.ImageName}",
					Status = pr.Status,
				})
				.OrderBy(pr => pr.StartDate)
				.ToList();

			return Ok(eventAttributes);
		}
		[HttpGet("GetRunsResult")]
		public IActionResult GetRunsResult([FromQuery] string email)
		{
			if (string.IsNullOrEmpty(email))
			{
				return BadRequest("Email cannot be empty.");
			}

			var participant = _db.Participants.FirstOrDefault(p => p.Email == email);

			if (participant == null)
			{
				return NotFound("Participant not found.");
			}

			var participantId = participant.Id;
			var eventAttributes = _db.Participantsruns
		.Where(pr => pr.ParticipantFK == participantId && pr.Status.ToLower() == "completed")
				.Select(pr => new
				{
					EventName = pr.EventAttribute.Event.EventName,
					EventType = pr.EventAttribute.EventType.Type,
					StartDate = pr.EventAttribute.Event.Start.Date,
					StartTime = pr.EventAttribute.Event.Start.TimeOfDay,
					StartPositionLongitude = pr.StartPositionLongitude,
					StartPositionLatitude = pr.StartPositionLatitude,
					EndPositionLongitude = pr.EndPositionLongitude,
					EndPositionLatitude = pr.EndPositionLatitude,
					Calories = pr.Calories,
					Time = pr.Time,
					Pace = pr.Pace,
				})
				.OrderBy(pr => pr.StartDate)
				.ToList();

			return Ok(eventAttributes);
		}

		[HttpPut]
		public IActionResult UpdateRun(string email, int eventAttributeFk, string runDistance, TimeOnly value)
		{
			int id = GetIdFromEmail(email);
			if (id == 0)
			{
				return BadRequest("This email doesn't exist");
			}
			var obj = _db.Participantsruns
				.FirstOrDefault(pr => pr.ParticipantFK == id && pr.EventAttributeFK == eventAttributeFk);

			if (obj != null)
			{
				switch (runDistance.ToLower())
				{
					case "start":
						obj.Start = value;
						_db.Participantsruns.Update(obj);
						_db.SaveChanges();
						return Ok("Start time updated successfully.");
					case "10":
						obj.Run_10 = value;
						_db.Participantsruns.Update(obj);
						_db.SaveChanges();
						return Ok("10Km time updated successfully.");
					case "20":
						obj.Run_20 = value;
						_db.Participantsruns.Update(obj);
						_db.SaveChanges();
						return Ok("20Km time updated successfully.");
					case "halfmarathon":
						obj.Run_HM = value;
						_db.Participantsruns.Update(obj);
						_db.SaveChanges();
						return Ok("Half marathon time updated successfully.");
					case "30":
						obj.Run_30 = value;
						_db.Participantsruns.Update(obj);
						_db.SaveChanges();
						return Ok("30Km time updated successfully.");
					case "40":
						obj.Run_40 = value;
						_db.Participantsruns.Update(obj);
						_db.SaveChanges();
						return Ok("40Km time updated successfully.");
					case "marathon":
						obj.Run_M = value;
						_db.Participantsruns.Update(obj);
						_db.SaveChanges();
						return Ok("Marathon time updated successfully.");
					default:
						return BadRequest("Invalid run distance.");
				}
			}
			else
			{
				return NotFound("ParticipantRun not found.");
			}
		}
		[HttpPut("updateStartPosition")]
		public IActionResult UpdateStartPosition(string email, int eventAttributeFk, string startPositionLatitude, string startPositionLongitude)
		{
			int id = GetIdFromEmail(email);
			if (id == 0)
			{
				return BadRequest("This email doesn't exist");
			}
			var obj = _db.Participantsruns
				.FirstOrDefault(pr => pr.ParticipantFK == id && pr.EventAttributeFK == eventAttributeFk);
			if (obj != null)
			{
				obj.StartPositionLatitude = startPositionLatitude;
				obj.StartPositionLongitude = startPositionLongitude;
				

				_db.SaveChanges();
				return Ok("Start Position updated successfully.");

			}
			else
			{
				return NotFound("ParticipantRun not found.");
			}
		}

		[HttpPut("updateEndPosition")]
		public IActionResult UpdateEndPosition(string email, int eventAttributeFk, string endPositionLatitude, string endPositionLongitude, int distance, TimeOnly time)
		{
			var participant = _db.Participants.FirstOrDefault(p => p.Email == email);

			if (participant == null)
			{
				return BadRequest("This email doesn't exist");
			}

			var obj = _db.Participantsruns
				.FirstOrDefault(pr => pr.ParticipantFK == participant.Id && pr.EventAttributeFK == eventAttributeFk);

			if (obj == null)
			{
				return NotFound("ParticipantRun not found.");
			}

			obj.EndPositionLatitude = endPositionLatitude;
			obj.EndPositionLongitude = endPositionLongitude;
			// Get participant details
			var weight = participant.Weight;
			var height = participant.Height;
			var age = participant.Age;
			var gender = participant.Gender;

			// Update status, time, and pace
			obj.Status = "completed";
			obj.Time = time - obj.Start;
			obj.Pace = obj.Time / distance;

			// Calculate BMR using Mifflin-St Jeor Equation
			double? bmr;
			if (gender == "Female")
			{
				bmr = 10 * weight + 6.25 * height - 5 * age - 161;
			}
			else
			{
				bmr = 10 * weight + 6.25 * height - 5 * age + 5;
			}
			// MET value for running (assuming average running speed of 8 km/h)
			double met = 8.0;
			// Calculate calories burned per minute
			double? caloriesPerMinute = (bmr / 1440) * met;
			// Extract hours and minutes from obj.Time
			double totalMinutes = obj.Time.Hours * 60 + obj.Time.Minutes + (obj.Time.Seconds / 60.0);
			// Calculate total calories burned
			obj.Calories = caloriesPerMinute * totalMinutes;
			_db.SaveChanges();
			return Ok("End position updated successfully.");
		}

		private int GetIdFromEmail(string email)
		{
			var participant = _db.Participants.FirstOrDefault(p => p.Email == email);

			if (participant != null)
			{
				return participant.Id;
			}
			return 0;
			
		}
		
	}
}
