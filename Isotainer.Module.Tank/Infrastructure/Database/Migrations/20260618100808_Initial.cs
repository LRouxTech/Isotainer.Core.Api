using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Isotainer.Module.Tank.Infrastructure.Database.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "Tank");

            migrationBuilder.CreateTable(
                name: "Company",
                schema: "Tank",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    CreatedById = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedById = table.Column<Guid>(type: "uuid", nullable: false),
                    UpdatedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ArchivedById = table.Column<Guid>(type: "uuid", nullable: true),
                    ArchivedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Company", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "WashStatus",
                schema: "Tank",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Type = table.Column<int>(type: "integer", maxLength: 200, nullable: false),
                    CreatedById = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedById = table.Column<Guid>(type: "uuid", nullable: false),
                    UpdatedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ArchivedById = table.Column<Guid>(type: "uuid", nullable: true),
                    ArchivedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WashStatus", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "IsotainerTank",
                schema: "Tank",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    TankNumber = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    WashStatusId = table.Column<Guid>(type: "uuid", nullable: false),
                    CompanyId = table.Column<Guid>(type: "uuid", nullable: false),
                    LoadedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UnloadedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    CreatedById = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedById = table.Column<Guid>(type: "uuid", nullable: false),
                    UpdatedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ArchivedById = table.Column<Guid>(type: "uuid", nullable: true),
                    ArchivedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IsotainerTank", x => x.Id);
                    table.ForeignKey(
                        name: "FK_IsotainerTank_Company_CompanyId",
                        column: x => x.CompanyId,
                        principalSchema: "Tank",
                        principalTable: "Company",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_IsotainerTank_WashStatus_WashStatusId",
                        column: x => x.WashStatusId,
                        principalSchema: "Tank",
                        principalTable: "WashStatus",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Company_Name",
                schema: "Tank",
                table: "Company",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_IsotainerTank_CompanyId",
                schema: "Tank",
                table: "IsotainerTank",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_IsotainerTank_TankNumber",
                schema: "Tank",
                table: "IsotainerTank",
                column: "TankNumber",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_IsotainerTank_WashStatusId",
                schema: "Tank",
                table: "IsotainerTank",
                column: "WashStatusId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "IsotainerTank",
                schema: "Tank");

            migrationBuilder.DropTable(
                name: "Company",
                schema: "Tank");

            migrationBuilder.DropTable(
                name: "WashStatus",
                schema: "Tank");
        }
    }
}
