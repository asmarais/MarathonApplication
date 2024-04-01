using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace MarathonApplication.Models
{
	public class EventType
	{
		[Key]
		public int Id { get; set; }
		public string Type { get; set; }
		[JsonIgnore]
		public ICollection<EventAttribute>? EventAttributes { get; set; }
	}
}
