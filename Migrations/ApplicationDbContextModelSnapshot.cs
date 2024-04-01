﻿// <auto-generated />
using System;
using MarathonApplication.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace MarathonApplication.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    partial class ApplicationDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "9.0.0-preview.1.24081.2")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("MarathonApplication.Models.Event", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int?>("DaysBeforeTheEvent")
                        .HasColumnType("int");

                    b.Property<DateTime>("End")
                        .HasColumnType("datetime2");

                    b.Property<string>("EventName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("MaximumParticipants")
                        .HasColumnType("int");

                    b.Property<string>("Remarks")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("Start")
                        .HasColumnType("datetime2");

                    b.Property<string>("Status")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Events");
                });

            modelBuilder.Entity("MarathonApplication.Models.EventAttribute", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("EventFK")
                        .HasColumnType("int");

                    b.Property<int>("EventTypeFK")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("EventFK");

                    b.HasIndex("EventTypeFK");

                    b.ToTable("EventAttributes");
                });

            modelBuilder.Entity("MarathonApplication.Models.EventType", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Type")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("EventTypes");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Type = "10km"
                        },
                        new
                        {
                            Id = 2,
                            Type = "Half Marathon"
                        },
                        new
                        {
                            Id = 3,
                            Type = "Marathon"
                        });
                });

            modelBuilder.Entity("MarathonApplication.Models.Participant", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("BirthPlace")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateOnly>("Birthday")
                        .HasColumnType("date");

                    b.Property<string>("City")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Country")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("EmergencyMobile")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("EmergencyPerson")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Gender")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("NbMarathon")
                        .HasColumnType("int");

                    b.Property<string>("PasswordHash")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Phone")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Remarks")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("SecondName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Street")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ZipCode")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Participants");
                });

            modelBuilder.Entity("MarathonApplication.Models.Participantsrun", b =>
                {
                    b.Property<int>("EventAttributeFK")
                        .HasColumnType("int");

                    b.Property<int>("ParticipantFK")
                        .HasColumnType("int");

                    b.Property<int>("Id")
                        .HasColumnType("int");

                    b.Property<DateTime>("Registration")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("Run_10")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("Run_20")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("Run_30")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("Run_40")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("Run_HM")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("Run_M")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("Start")
                        .HasColumnType("datetime2");

                    b.Property<string>("Status")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("TShirtSize")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("EventAttributeFK", "ParticipantFK");

                    b.HasIndex("ParticipantFK");

                    b.ToTable("Participantsruns");
                });

            modelBuilder.Entity("MarathonApplication.Models.EventAttribute", b =>
                {
                    b.HasOne("MarathonApplication.Models.Event", "Event")
                        .WithMany("EventAttributes")
                        .HasForeignKey("EventFK")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("MarathonApplication.Models.EventType", "EventType")
                        .WithMany("EventAttributes")
                        .HasForeignKey("EventTypeFK")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Event");

                    b.Navigation("EventType");
                });

            modelBuilder.Entity("MarathonApplication.Models.Participantsrun", b =>
                {
                    b.HasOne("MarathonApplication.Models.EventAttribute", "EventAttribute")
                        .WithMany("Participantsruns")
                        .HasForeignKey("EventAttributeFK")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("MarathonApplication.Models.Participant", "Participant")
                        .WithMany("Participantsruns")
                        .HasForeignKey("ParticipantFK")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("EventAttribute");

                    b.Navigation("Participant");
                });

            modelBuilder.Entity("MarathonApplication.Models.Event", b =>
                {
                    b.Navigation("EventAttributes");
                });

            modelBuilder.Entity("MarathonApplication.Models.EventAttribute", b =>
                {
                    b.Navigation("Participantsruns");
                });

            modelBuilder.Entity("MarathonApplication.Models.EventType", b =>
                {
                    b.Navigation("EventAttributes");
                });

            modelBuilder.Entity("MarathonApplication.Models.Participant", b =>
                {
                    b.Navigation("Participantsruns");
                });
#pragma warning restore 612, 618
        }
    }
}
