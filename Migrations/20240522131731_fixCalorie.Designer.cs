﻿// <auto-generated />
using System;
using MarathonApplication.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace MarathonApplication.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20240522131731_fixCalorie")]
    partial class fixCalorie
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
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

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("End")
                        .HasColumnType("datetime2");

                    b.Property<string>("EventName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ImageName")
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

                    b.Property<DateOnly>("Birthday")
                        .HasColumnType("date");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Gender")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PasswordHash")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Phone")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("RefreshToken")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("TokenExpiryDate")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.ToTable("Participants");
                });

            modelBuilder.Entity("MarathonApplication.Models.Participantsrun", b =>
                {
                    b.Property<int>("EventAttributeFK")
                        .HasColumnType("int");

                    b.Property<int>("ParticipantFK")
                        .HasColumnType("int");

                    b.Property<double?>("Calories")
                        .HasColumnType("float");

                    b.Property<string>("EndPositionLatitude")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("EndPositionLongitude")
                        .HasColumnType("nvarchar(max)");

                    b.Property<TimeSpan?>("Pace")
                        .HasColumnType("time");

                    b.Property<DateTime>("Registration")
                        .HasColumnType("datetime2");

                    b.Property<TimeOnly?>("Run_10")
                        .HasColumnType("time");

                    b.Property<TimeOnly?>("Run_20")
                        .HasColumnType("time");

                    b.Property<TimeOnly?>("Run_30")
                        .HasColumnType("time");

                    b.Property<TimeOnly?>("Run_40")
                        .HasColumnType("time");

                    b.Property<TimeOnly?>("Run_HM")
                        .HasColumnType("time");

                    b.Property<TimeOnly?>("Run_M")
                        .HasColumnType("time");

                    b.Property<TimeOnly?>("Start")
                        .HasColumnType("time");

                    b.Property<string>("StartPositionLatitude")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("StartPositionLongitude")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("TShirtSize")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<TimeSpan?>("Time")
                        .HasColumnType("time");

                    b.Property<int>("Weight")
                        .HasColumnType("int");

                    b.Property<string>("status")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("EventAttributeFK", "ParticipantFK");

                    b.HasIndex("ParticipantFK");

                    b.ToTable("Participantsruns");
                });

            modelBuilder.Entity("MarathonApplication.Models.Role", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("role")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Roles");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            role = "Admin"
                        },
                        new
                        {
                            Id = 2,
                            role = "Moderator"
                        });
                });

            modelBuilder.Entity("MarathonApplication.Models.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("PasswordHash")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("RefreshToken")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("RoleId")
                        .HasColumnType("int");

                    b.Property<DateTime?>("TokenExpiryDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Username")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("Users");
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

            modelBuilder.Entity("MarathonApplication.Models.User", b =>
                {
                    b.HasOne("MarathonApplication.Models.Role", "Role")
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Role");
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
