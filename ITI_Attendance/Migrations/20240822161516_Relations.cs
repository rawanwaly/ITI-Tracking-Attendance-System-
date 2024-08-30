using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ITI_Attendance.Migrations
{
    /// <inheritdoc />
    public partial class Relations : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "DeptNum",
                table: "Students",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "HrId",
                table: "Students",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "StudentId",
                table: "StudentAttendances",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "DepartmentProgram",
                columns: table => new
                {
                    DepartmentsId = table.Column<int>(type: "int", nullable: false),
                    ProgramsId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DepartmentProgram", x => new { x.DepartmentsId, x.ProgramsId });
                    table.ForeignKey(
                        name: "FK_DepartmentProgram_Departments_DepartmentsId",
                        column: x => x.DepartmentsId,
                        principalTable: "Departments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DepartmentProgram_Programs_ProgramsId",
                        column: x => x.ProgramsId,
                        principalTable: "Programs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Intakes",
                columns: table => new
                {
                    StudentId = table.Column<int>(type: "int", nullable: false),
                    ProgramId = table.Column<int>(type: "int", nullable: false),
                    Intake = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Intakes", x => new { x.StudentId, x.ProgramId });
                    table.ForeignKey(
                        name: "FK_Intakes_Programs_ProgramId",
                        column: x => x.ProgramId,
                        principalTable: "Programs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Intakes_Students_StudentId",
                        column: x => x.StudentId,
                        principalTable: "Students",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ProgramStudent",
                columns: table => new
                {
                    ProgramsId = table.Column<int>(type: "int", nullable: false),
                    StudentsId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProgramStudent", x => new { x.ProgramsId, x.StudentsId });
                    table.ForeignKey(
                        name: "FK_ProgramStudent_Programs_ProgramsId",
                        column: x => x.ProgramsId,
                        principalTable: "Programs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProgramStudent_Students_StudentsId",
                        column: x => x.StudentsId,
                        principalTable: "Students",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserRoles",
                columns: table => new
                {
                    UserLoginId = table.Column<int>(type: "int", nullable: false),
                    RoleId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserRoles", x => new { x.UserLoginId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_UserRoles_Roles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "Roles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserRoles_UserLogins_UserLoginId",
                        column: x => x.UserLoginId,
                        principalTable: "UserLogins",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Students_DeptNum",
                table: "Students",
                column: "DeptNum");

            migrationBuilder.CreateIndex(
                name: "IX_Students_HrId",
                table: "Students",
                column: "HrId");

            migrationBuilder.CreateIndex(
                name: "IX_StudentAttendances_StudentId",
                table: "StudentAttendances",
                column: "StudentId");

            migrationBuilder.CreateIndex(
                name: "IX_DepartmentProgram_ProgramsId",
                table: "DepartmentProgram",
                column: "ProgramsId");

            migrationBuilder.CreateIndex(
                name: "IX_Intakes_ProgramId",
                table: "Intakes",
                column: "ProgramId");

            migrationBuilder.CreateIndex(
                name: "IX_ProgramStudent_StudentsId",
                table: "ProgramStudent",
                column: "StudentsId");

            migrationBuilder.CreateIndex(
                name: "IX_UserRoles_RoleId",
                table: "UserRoles",
                column: "RoleId");

            migrationBuilder.AddForeignKey(
                name: "FK_StudentAttendances_Students_StudentId",
                table: "StudentAttendances",
                column: "StudentId",
                principalTable: "Students",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Students_Departments_DeptNum",
                table: "Students",
                column: "DeptNum",
                principalTable: "Departments",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Students_Hrs_HrId",
                table: "Students",
                column: "HrId",
                principalTable: "Hrs",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_StudentAttendances_Students_StudentId",
                table: "StudentAttendances");

            migrationBuilder.DropForeignKey(
                name: "FK_Students_Departments_DeptNum",
                table: "Students");

            migrationBuilder.DropForeignKey(
                name: "FK_Students_Hrs_HrId",
                table: "Students");

            migrationBuilder.DropTable(
                name: "DepartmentProgram");

            migrationBuilder.DropTable(
                name: "Intakes");

            migrationBuilder.DropTable(
                name: "ProgramStudent");

            migrationBuilder.DropTable(
                name: "UserRoles");

            migrationBuilder.DropIndex(
                name: "IX_Students_DeptNum",
                table: "Students");

            migrationBuilder.DropIndex(
                name: "IX_Students_HrId",
                table: "Students");

            migrationBuilder.DropIndex(
                name: "IX_StudentAttendances_StudentId",
                table: "StudentAttendances");

            migrationBuilder.DropColumn(
                name: "DeptNum",
                table: "Students");

            migrationBuilder.DropColumn(
                name: "HrId",
                table: "Students");

            migrationBuilder.DropColumn(
                name: "StudentId",
                table: "StudentAttendances");
        }
    }
}
