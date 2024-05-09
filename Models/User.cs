using System.ComponentModel.DataAnnotations;
using System.Data;

namespace MarathonApplication.Models
{
	public class User
	{
		[Key]
		public int Id { get; set; }
		public string Username { get; set; } = string.Empty;
		public string PasswordHash { get; set; } = string.Empty;
		public Role Role { get; set; }
		public string RefreshToken { get; set; } = string.Empty;
		public DateTime? TokenExpiryDate { get; set; } 


		public User(string username, string password, Role role)
        {
			Username = username;
			PasswordHash = password;
			Role = role;
        }
        public User()
        {
            
        }
    }
}
