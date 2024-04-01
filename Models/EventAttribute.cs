using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace MarathonApplication.Models
{
	public class EventAttribute
	{
		[Key]
		public int Id { get; set; }
		public int EventTypeFK { get; set; }
		public int EventFK { get; set; }
		[NotMapped] // This property will not be mapped to the database
		public bool IsSelected { get; set; }
		[JsonIgnore]
		public Event? Event { get; set; }
		[JsonIgnore]

		public EventType? EventType { get; set; }
		public ICollection<Participantsrun>? Participantsruns { get; set; }
	}
}
