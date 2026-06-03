using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProjectService.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddProjectEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsManager",
                table: "Employees");

            migrationBuilder.CreateTable(
                name: "Projects",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    CustomerCompanyName = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    ExecutorCompanyName = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    StartDate = table.Column<DateOnly>(type: "date", nullable: false),
                    EndDate = table.Column<DateOnly>(type: "date", nullable: false),
                    Priority = table.Column<int>(type: "integer", nullable: false),
                    ManagerId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Projects", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Projects_Employees_ManagerId",
                        column: x => x.ManagerId,
                        principalTable: "Employees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ProjectEmployees",
                columns: table => new
                {
                    EmployeesId = table.Column<Guid>(type: "uuid", nullable: false),
                    ProjectId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProjectEmployees", x => new { x.EmployeesId, x.ProjectId });
                    table.ForeignKey(
                        name: "FK_ProjectEmployees_Employees_EmployeesId",
                        column: x => x.EmployeesId,
                        principalTable: "Employees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProjectEmployees_Projects_ProjectId",
                        column: x => x.ProjectId,
                        principalTable: "Projects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ProjectEmployees_ProjectId",
                table: "ProjectEmployees",
                column: "ProjectId");

            migrationBuilder.CreateIndex(
                name: "IX_Projects_ManagerId",
                table: "Projects",
                column: "ManagerId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProjectEmployees");

            migrationBuilder.DropTable(
                name: "Projects");

            migrationBuilder.AddColumn<bool>(
                name: "IsManager",
                table: "Employees",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }
    }
}
