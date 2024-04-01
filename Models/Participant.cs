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
		public string SecondName { get; set; }
		public string? Country { get; set; }
		public string? ZipCode { get; set; }
		public string? City { get; set; }
		public string? Street { get; set; }
		public DateOnly Birthday { get; set; } = new DateOnly(2000, 1, 1);
		public string BirthPlace { get; set; }
		public string Gender { get; set; }
		public string Email { get; set; }
		public string PasswordHash { get; set; }
		public string Phone { get; set; }
		public int? NbMarathon { get; set; }
		public string? EmergencyPerson { get; set; }
		public string? EmergencyMobile { get; set; }
		public string? Remarks { get; set; }
		[JsonIgnore]
		public ICollection<Participantsrun>? Participantsruns { get; set; }
		public Participant(
		string firstName,
		string secondName,
		string? country,
		string? zipCode,
		string? city,
		string? street,
		DateOnly birthday,
		string birthPlace,
		string gender,
		string email,
		string phone,
		int? nbMarathon,
		string? emergencyPerson,
		string? emergencyMobile,
		string? remarks,
		string passwordHash)
		{
			FirstName = firstName;
			SecondName = secondName;
			Country = country;
			ZipCode = zipCode;
			City = city;
			Street = street;
			Birthday = birthday;
			BirthPlace = birthPlace;
			Gender = gender;
			Email = email;
			Phone = phone;
			NbMarathon = nbMarathon;
			EmergencyPerson = emergencyPerson;
			EmergencyMobile = emergencyMobile;
			Remarks = remarks;
			PasswordHash = passwordHash;
		}
        public Participant()
        {
            
        }
    }
}
