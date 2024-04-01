namespace MarathonApplication.Models
{
	public class UserDto
	{
		public required int Id { get; set; }
		public required string UserName { get; set; }
		public required string PasswordHash { get; set; }
	}
}
