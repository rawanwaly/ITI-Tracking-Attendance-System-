using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ITI_Attendance.Migrations
{
    /// <inheritdoc />
    public partial class Degree : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Degree",
                table: "Intakes",
                type: "int",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Degree",
                table: "Intakes");
        }
    }
}
