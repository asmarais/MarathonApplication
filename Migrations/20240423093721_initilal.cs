using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace MarathonApplication.Migrations
{
    /// <inheritdoc />
    public partial class initilal : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Events",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EventName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Start = table.Column<DateTime>(type: "datetime2", nullable: false),
                    End = table.Column<DateTime>(type: "datetime2", nullable: false),
                    MaximumParticipants = table.Column<int>(type: "int", nullable: true),
                    DaysBeforeTheEvent = table.Column<int>(type: "int", nullable: true),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Events", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "EventTypes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Type = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EventTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Participants",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FirstName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Country = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ZipCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    City = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Street = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Birthday = table.Column<DateOnly>(type: "date", nullable: false),
                    Gender = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Phone = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Participants", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "RefreshTokens",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    Token = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ExpiryDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RefreshTokens", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Roles",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    role = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Roles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "EventAttributes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EventTypeFK = table.Column<int>(type: "int", nullable: false),
                    EventFK = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EventAttributes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EventAttributes_EventTypes_EventTypeFK",
                        column: x => x.EventTypeFK,
                        principalTable: "EventTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EventAttributes_Events_EventFK",
                        column: x => x.EventFK,
                        principalTable: "Events",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Username = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RoleId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Users_Roles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "Roles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Participantsruns",
                columns: table => new
                {
                    EventAttributeFK = table.Column<int>(type: "int", nullable: false),
                    ParticipantFK = table.Column<int>(type: "int", nullable: false),
                    Id = table.Column<int>(type: "int", nullable: false),
                    Registration = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Start = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Run_10 = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Run_20 = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Run_HM = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Run_30 = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Run_40 = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Run_M = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TShirtSize = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Participantsruns", x => new { x.EventAttributeFK, x.ParticipantFK });
                    table.ForeignKey(
                        name: "FK_Participantsruns_EventAttributes_EventAttributeFK",
                        column: x => x.EventAttributeFK,
                        principalTable: "EventAttributes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Participantsruns_Participants_ParticipantFK",
                        column: x => x.ParticipantFK,
                        principalTable: "Participants",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "EventTypes",
                columns: new[] { "Id", "Type" },
                values: new object[,]
                {
                    { 1, "10km" },
                    { 2, "Half Marathon" },
                    { 3, "Marathon" }
                });

            migrationBuilder.InsertData(
                table: "Roles",
                columns: new[] { "Id", "role" },
                values: new object[,]
                {
                    { 1, "Admin" },
                    { 2, "Moderator" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_EventAttributes_EventFK",
                table: "EventAttributes",
                column: "EventFK");

            migrationBuilder.CreateIndex(
                name: "IX_EventAttributes_EventTypeFK",
                table: "EventAttributes",
                column: "EventTypeFK");

            migrationBuilder.CreateIndex(
                name: "IX_Participantsruns_ParticipantFK",
                table: "Participantsruns",
                column: "ParticipantFK");

            migrationBuilder.CreateIndex(
                name: "IX_Users_RoleId",
                table: "Users",
                column: "RoleId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Participantsruns");

            migrationBuilder.DropTable(
                name: "RefreshTokens");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "EventAttributes");

            migrationBuilder.DropTable(
                name: "Participants");

            migrationBuilder.DropTable(
                name: "Roles");

            migrationBuilder.DropTable(
                name: "EventTypes");

            migrationBuilder.DropTable(
                name: "Events");
        }
    }
}
