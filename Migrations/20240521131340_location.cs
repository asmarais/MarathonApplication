using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MarathonApplication.Migrations
{
    /// <inheritdoc />
    public partial class location : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "StartPosition",
                table: "Participantsruns",
                newName: "StartPositionLongitude");

            migrationBuilder.RenameColumn(
                name: "EndPosition",
                table: "Participantsruns",
                newName: "StartPositionLatitude");

            migrationBuilder.AddColumn<string>(
                name: "EndPositionLatitude",
                table: "Participantsruns",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "EndPositionLongitude",
                table: "Participantsruns",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EndPositionLatitude",
                table: "Participantsruns");

            migrationBuilder.DropColumn(
                name: "EndPositionLongitude",
                table: "Participantsruns");

            migrationBuilder.RenameColumn(
                name: "StartPositionLongitude",
                table: "Participantsruns",
                newName: "StartPosition");

            migrationBuilder.RenameColumn(
                name: "StartPositionLatitude",
                table: "Participantsruns",
                newName: "EndPosition");
        }
    }
}
