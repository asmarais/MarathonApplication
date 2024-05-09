using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace MarathonApplication.Models
{
	public class ParticipantDto
		{
			public string FirstName { get; set; }
			public string SecondName { get; set; }
		/*
			public string? Country { get; set; }
			public string? ZipCode { get; set; }
			public string? City { get; set; }
			public string? Street { get; set; }
		*/
			public DateOnly Birthday { get; set; }
		/*
			public string BirthPlace { get; set; }
			public string Gender { get; set; }
		*/
			public string Email { get; set; }
			public string Password { get; set; }
			public string Phone { get; set; }
			
			[JsonIgnore]
			public ICollection<Participantsrun>? Participantsruns { get; set; }
	}
}


