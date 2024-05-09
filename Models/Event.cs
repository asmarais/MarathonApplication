using Microsoft.Extensions.Hosting;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace MarathonApplication.Models
{
    public class Event
    {
        [Key]
        public int Id { get; set; }
        public string EventName { get; set; }
        public string Description { get; set; }    
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
		public string Status { get; set; }
        public string? ImageName { get; set; }
        [NotMapped]
		public IFormFile ImageFile { get; set; }
		[JsonIgnore]
		public ICollection<EventAttribute>? EventAttributes { get; } = new List<EventAttribute>();
	}
}
