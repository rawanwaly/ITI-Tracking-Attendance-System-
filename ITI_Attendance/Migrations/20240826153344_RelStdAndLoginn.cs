using Microsoft.EntityFrameworkCore.Migrations;

namespace ITI_Attendance.Migrations
{
    public partial class RelStdAndLoginn : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Add UserLoginId column to Students table, allowing nulls initially
            migrationBuilder.AddColumn<int>(
                name: "UserLoginId",
                table: "Students",
                type: "int",
                nullable: true);  // Allow nulls to avoid conflicts during migration

            // Create index on UserLoginId
            migrationBuilder.CreateIndex(
                name: "IX_Students_UserLoginId",
                table: "Students",
                column: "UserLoginId");

            // Add foreign key constraint
            migrationBuilder.AddForeignKey(
                name: "FK_Students_UserLogins_UserLoginId",
                table: "Students",
                column: "UserLoginId",
                principalTable: "UserLogins",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);  // Use Restrict to avoid deletion issues
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Drop foreign key constraint
            migrationBuilder.DropForeignKey(
                name: "FK_Students_UserLogins_UserLoginId",
                table: "Students");

            // Drop index on UserLoginId
            migrationBuilder.DropIndex(
                name: "IX_Students_UserLoginId",
                table: "Students");

            // Drop UserLoginId column
            migrationBuilder.DropColumn(
                name: "UserLoginId",
                table: "Students");
        }
    }
}
