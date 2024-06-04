using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace MarathonApplication.Models
{
	public class Participantsrun
	{
		public DateTime Registration { get; set; } = DateTime.Now;
		public TimeOnly Start { get; set; } = TimeOnly.MinValue;
		public TimeOnly? Run_10 { get; set; }
		public TimeOnly? Run_20 { get; set; }
		public TimeOnly? Run_HM { get; set; }
		public TimeOnly? Run_30 { get; set; }
		public TimeOnly? Run_40 { get; set; }
		public TimeOnly? Run_M { get; set; }
		public double? Calories { get; set; }
		public string? StartPositionLatitude { get; set; }
		public string? StartPositionLongitude { get; set; }
		public string? EndPositionLatitude { get; set; }
		public string? EndPositionLongitude { get; set; }
		public TimeSpan Time {  get; set; } = TimeSpan.Zero;
		public TimeSpan? Pace { get; set; }
		public int EventAttributeFK { get; set; }
		public string? Status { get; set; }
		[JsonIgnore]
		public EventAttribute? EventAttribute { get; set; }
		public int ParticipantFK { get; set; }
		[JsonIgnore]

		public Participant? Participant { get; set; }
    }
}
