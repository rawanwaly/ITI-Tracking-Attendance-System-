using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ITI_Attendance.Migrations
{
    /// <inheritdoc />
    public partial class @null : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Students_UserLogins_UserLoginId",
                table: "Students");

            migrationBuilder.DropIndex(
                name: "IX_Students_UserLoginId",
                table: "Students");

            migrationBuilder.AlterColumn<int>(
                name: "UserLoginId",
                table: "Students",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.CreateIndex(
                name: "IX_Students_UserLoginId",
                table: "Students",
                column: "UserLoginId",
                unique: true,
                filter: "[UserLoginId] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_Students_UserLogins_UserLoginId",
                table: "Students",
                column: "UserLoginId",
                principalTable: "UserLogins",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Students_UserLogins_UserLoginId",
                table: "Students");

            migrationBuilder.DropIndex(
                name: "IX_Students_UserLoginId",
                table: "Students");

            migrationBuilder.AlterColumn<int>(
                name: "UserLoginId",
                table: "Students",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Students_UserLoginId",
                table: "Students",
                column: "UserLoginId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Students_UserLogins_UserLoginId",
                table: "Students",
                column: "UserLoginId",
                principalTable: "UserLogins",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
