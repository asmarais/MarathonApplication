using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace MarathonApplication.Models
{
	public class ParticipantDto
		{
			public string FirstName { get; set; }
			public string SecondName { get; set; }
			public string Email { get; set; }
			public string Password { get; set; }
			public string Phone { get; set; }
			
			[JsonIgnore]
			public ICollection<Participantsrun>? Participantsruns { get; set; }
	}
}


