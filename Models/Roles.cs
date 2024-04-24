using System.ComponentModel.DataAnnotations;

namespace MarathonApplication.Models
{
	public class Role
	{
		[Key]
		public int Id { get; set; }
		public string role { get; set; }= string.Empty;

	}
}
