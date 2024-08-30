using Microsoft.EntityFrameworkCore.Migrations;

namespace ITI_Attendance.Migrations
{
    public partial class RelBetweenLoginHr : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Add UserLoginId column to Hrs table
            migrationBuilder.AddColumn<int>(
                name: "UserLoginId",
                table: "Hrs",
                type: "int",
                nullable: true);  // Allow nulls initially to avoid conflict

            // Create index on UserLoginId
            migrationBuilder.CreateIndex(
                name: "IX_Hrs_UserLoginId",
                table: "Hrs",
                column: "UserLoginId");

            // Add foreign key constraint
            migrationBuilder.AddForeignKey(
                name: "FK_Hrs_UserLogins_UserLoginId",
                table: "Hrs",
                column: "UserLoginId",
                principalTable: "UserLogins",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);  // Set to Restrict to avoid deletion issues
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Drop foreign key constraint
            migrationBuilder.DropForeignKey(
                name: "FK_Hrs_UserLogins_UserLoginId",
                table: "Hrs");

            // Drop index on UserLoginId
            migrationBuilder.DropIndex(
                name: "IX_Hrs_UserLoginId",
                table: "Hrs");

            // Drop UserLoginId column
            migrationBuilder.DropColumn(
                name: "UserLoginId",
                table: "Hrs");
        }
    }
}
