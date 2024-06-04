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
        [NotMapped]
        public string ImageSrc { get; set; }
		[JsonIgnore]
		public ICollection<EventAttribute>? EventAttributes { get; } = new List<EventAttribute>();
        public Event(int id, string eventName, string desciption, DateTime start, DateTime end, string status, string imageName, string imageSrc)
        {
            Id = id;
            EventName = eventName;
            Description = desciption;
            Start = start;
            End = end;
            Status = status;
            ImageName = imageName;
            ImageSrc = imageSrc;
        }
        public Event()
        {
            
        }
    }
}
