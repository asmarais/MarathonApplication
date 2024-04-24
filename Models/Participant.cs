using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;

namespace MarathonApplication.Models
{
	public class Participant
	{
		[Key]
		public int Id { get; set; }
		public string FirstName { get; set; }
		public string LastName { get; set; }
		public string? Country { get; set; }
		public string? ZipCode { get; set; }
		public string? City { get; set; }
		public string? Street { get; set; }
		public DateOnly Birthday { get; set; } 
		public string Gender { get; set; }
		public string Email { get; set; }
		public string PasswordHash { get; set; }
		public string Phone { get; set; }
		
		
		[JsonIgnore]
		public ICollection<Participantsrun>? Participantsruns { get; set; }
		public Participant(
		string firstName,
		string lastName,
		string? country,
		string? zipCode,
		string? city,
		string? street,
		DateOnly birthday,
		string gender,
		string email,
		string phone,
		
		string passwordHash)
		{
			FirstName = firstName;
			LastName = lastName;
			Country = country;
			ZipCode = zipCode;
			City = city;
			Street = street;
			Birthday = birthday;
			Gender = gender;
			Email = email;
			Phone = phone;
			PasswordHash = passwordHash;
		}
        public Participant()
        {
            
        }
    }
}
