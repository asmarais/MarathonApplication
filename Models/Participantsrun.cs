using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace MarathonApplication.Models
{
	public class Participantsrun
	{
		[Key]
		public int Id { get; set; }
		public DateTime Registration { get; set; } = DateTime.Now;

		[DisplayName("Start")]
		public DateTime Start { get; set; }
		[DisplayName("RUN 10")]
		public DateTime? Run_10 { get; set; }
		[DisplayName("RUN 20")]
		public DateTime? Run_20 { get; set; }
		[DisplayName("RUN Half Marathon")]
		public DateTime? Run_HM { get; set; }
		[DisplayName("RUN 30")]
		public DateTime? Run_30 { get; set; }
		[DisplayName("RUN 40")]
		public DateTime? Run_40 { get; set; }
		[DisplayName("RUN Marathon")]
		public DateTime? Run_M { get; set; }
		[DisplayName("Status")]
		public string Status { get; set; }
		public string? TShirtSize { get; set; }
		
		public int EventAttributeFK { get; set; }
		[JsonIgnore]

		public EventAttribute? EventAttribute { get; set; }
		public int ParticipantFK { get; set; }
		[JsonIgnore]

		public Participant? Participant { get; set; }
    }
}
