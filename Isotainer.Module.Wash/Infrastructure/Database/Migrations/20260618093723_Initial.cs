using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Isotainer.Module.Wash.Infrastructure.Database.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "WashType",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Type = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Cost = table.Column<double>(type: "double precision", precision: 2, nullable: false),
                    CreatedById = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedById = table.Column<Guid>(type: "uuid", nullable: false),
                    UpdatedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ArchivedById = table.Column<Guid>(type: "uuid", nullable: true),
                    ArchivedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WashType", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "WashInstruction",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    IsotainerTankId = table.Column<Guid>(type: "uuid", maxLength: 200, nullable: false),
                    WashTypeId = table.Column<Guid>(type: "uuid", maxLength: 200, nullable: false),
                    InstructedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    FinishedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    CreatedById = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedById = table.Column<Guid>(type: "uuid", nullable: false),
                    UpdatedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ArchivedById = table.Column<Guid>(type: "uuid", nullable: true),
                    ArchivedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WashInstruction", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WashInstruction_WashType_WashTypeId",
                        column: x => x.WashTypeId,
                        principalTable: "WashType",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_WashInstruction_WashTypeId",
                table: "WashInstruction",
                column: "WashTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_WashType_Type",
                table: "WashType",
                column: "Type",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "WashInstruction");

            migrationBuilder.DropTable(
                name: "WashType");
        }
    }
}
