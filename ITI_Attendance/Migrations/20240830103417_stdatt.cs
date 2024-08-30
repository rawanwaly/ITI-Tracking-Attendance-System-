using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ITI_Attendance.Migrations
{
    /// <inheritdoc />
    public partial class stdatt : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_StudentAttendances_StudentId",
                table: "StudentAttendances");

            migrationBuilder.CreateIndex(
                name: "IX_StudentAttendances_StudentId",
                table: "StudentAttendances",
                column: "StudentId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_StudentAttendances_StudentId",
                table: "StudentAttendances");

            migrationBuilder.CreateIndex(
                name: "IX_StudentAttendances_StudentId",
                table: "StudentAttendances",
                column: "StudentId");
        }
    }
}
