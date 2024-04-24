using Microsoft.EntityFrameworkCore;
using MarathonApplication.Models;
using System.Data;


namespace MarathonApplication.Data
{
	public class ApplicationDbContext : DbContext
	{
		public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
		{

		}
		public DbSet<Participantsrun> Participantsruns { get; set; }
		public DbSet<Participant> Participants { get; set; }
		public DbSet<Event> Events { get; set; }
		public DbSet<EventAttribute> EventAttributes { get; set; }
		public DbSet<EventType> EventTypes { get; set; }
		public DbSet<RefreshToken> RefreshTokens { get; set; }
		public DbSet<User> Users { get; set; }
		public DbSet<Role> Roles { get; set; }
		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			base.OnModelCreating(modelBuilder);
			modelBuilder.Entity<EventType>().HasData(
				new EventType { Id = 1, Type = "10km" },
				new EventType { Id = 2, Type = "Half Marathon" },
				new EventType { Id = 3, Type = "Marathon" }
			);
			modelBuilder.Entity<Role>().HasData(
				new Role { Id = 1, role = "Admin"},
				new Role { Id = 2, role = "Moderator" }
				);
			modelBuilder.Entity<Participantsrun>()
				.HasKey(sc => new { sc.EventAttributeFK, sc.ParticipantFK });

			modelBuilder.Entity<Participantsrun>()
				.HasOne(sc => sc.EventAttribute)
				.WithMany(s => s.Participantsruns)
				.HasForeignKey(sc => sc.EventAttributeFK);

			modelBuilder.Entity<Participantsrun>()
				.HasOne(sc => sc.Participant)
				.WithMany(c => c.Participantsruns)
				.HasForeignKey(sc => sc.ParticipantFK);

			modelBuilder.Entity<EventAttribute>()
			.HasOne(ea => ea.Event)
			.WithMany(e => e.EventAttributes)
			.HasForeignKey(ea => ea.EventFK);

			modelBuilder.Entity<EventAttribute>()
			.HasOne(ea => ea.EventType)
			.WithMany(e => e.EventAttributes)
			.HasForeignKey(ea => ea.EventTypeFK);
		}
	}
}

