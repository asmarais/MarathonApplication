using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MarathonApplication.Migrations
{
    /// <inheritdoc />
    public partial class fixCalories : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TShirtSize",
                table: "Participantsruns");

            migrationBuilder.DropColumn(
                name: "Weight",
                table: "Participantsruns");

            migrationBuilder.DropColumn(
                name: "Birthday",
                table: "Participants");

            migrationBuilder.RenameColumn(
                name: "status",
                table: "Participantsruns",
                newName: "Status");

            migrationBuilder.AlterColumn<TimeSpan>(
                name: "Time",
                table: "Participantsruns",
                type: "time",
                nullable: false,
                defaultValue: new TimeSpan(0, 0, 0, 0, 0),
                oldClrType: typeof(TimeSpan),
                oldType: "time",
                oldNullable: true);

            migrationBuilder.AlterColumn<TimeOnly>(
                name: "Start",
                table: "Participantsruns",
                type: "time",
                nullable: false,
                defaultValue: new TimeOnly(0, 0, 0),
                oldClrType: typeof(TimeOnly),
                oldType: "time",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Age",
                table: "Participants",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Height",
                table: "Participants",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TshirtSize",
                table: "Participants",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Weight",
                table: "Participants",
                type: "int",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Age",
                table: "Participants");

            migrationBuilder.DropColumn(
                name: "Height",
                table: "Participants");

            migrationBuilder.DropColumn(
                name: "TshirtSize",
                table: "Participants");

            migrationBuilder.DropColumn(
                name: "Weight",
                table: "Participants");

            migrationBuilder.RenameColumn(
                name: "Status",
                table: "Participantsruns",
                newName: "status");

            migrationBuilder.AlterColumn<TimeSpan>(
                name: "Time",
                table: "Participantsruns",
                type: "time",
                nullable: true,
                oldClrType: typeof(TimeSpan),
                oldType: "time");

            migrationBuilder.AlterColumn<TimeOnly>(
                name: "Start",
                table: "Participantsruns",
                type: "time",
                nullable: true,
                oldClrType: typeof(TimeOnly),
                oldType: "time");

            migrationBuilder.AddColumn<string>(
                name: "TShirtSize",
                table: "Participantsruns",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "Weight",
                table: "Participantsruns",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateOnly>(
                name: "Birthday",
                table: "Participants",
                type: "date",
                nullable: false,
                defaultValue: new DateOnly(1, 1, 1));
        }
    }
}
