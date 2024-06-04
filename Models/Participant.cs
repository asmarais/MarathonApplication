using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace MarathonApplication.Models
{
	public class Participant
	{
		[Key]
		public int Id { get; set; }

		[Required]
		public string FirstName { get; set; }

		[Required]
		public string LastName { get; set; }

		public string? Gender { get; set; }

		public int? Age { get; set; }

		public int? Height { get; set; }

		public int? Weight { get; set; }

		public string? TshirtSize { get; set; }

		[Required]
		[EmailAddress]
		public string Email { get; set; }

		[Required]
		public string PasswordHash { get; set; }

		[Required]
		[Phone]
		public string Phone { get; set; }

		public string RefreshToken { get; set; } = string.Empty;
		public DateTime? TokenExpiryDate { get; set; }

		[JsonIgnore]
		public ICollection<Participantsrun>? Participantsruns { get; set; }

		public Participant(
			string firstName,
			string lastName,
			int? age,
			int? height,
			int? weight,
			string? tshirtSize,
			string email,
			string phone,
			string passwordHash,
			string? gender)
		{
			FirstName = firstName;
			LastName = lastName;
			Age = age;
			Height = height;
			Weight = weight;
			TshirtSize = tshirtSize;
			Email = email;
			Phone = phone;
			PasswordHash = passwordHash;
			Gender = gender;
		}

		public Participant(
			string firstName,
			string lastName,
			string email,
			string phone,
			string passwordHash)
		{
			FirstName = firstName;
			LastName = lastName;
			Email = email;
			Phone = phone;
			PasswordHash = passwordHash;
		}

		public Participant() { }
	}
}
