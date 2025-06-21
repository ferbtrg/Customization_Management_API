using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Customization_Management_API.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Customizations",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    Description = table.Column<string>(type: "TEXT", nullable: false),
                    Type = table.Column<int>(type: "INTEGER", nullable: false),
                    Price = table.Column<decimal>(type: "TEXT", precision: 18, scale: 2, nullable: false),
                    CreatedBy = table.Column<Guid>(type: "TEXT", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Customizations", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Units",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    DevelopmentName = table.Column<string>(type: "TEXT", nullable: false),
                    UnitNumber = table.Column<string>(type: "TEXT", nullable: false),
                    ClientName = table.Column<string>(type: "TEXT", nullable: false),
                    ClientCPF = table.Column<string>(type: "TEXT", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "TEXT", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Units", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CustomizationRequests",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    UnitId = table.Column<Guid>(type: "TEXT", nullable: false),
                    TotalValue = table.Column<decimal>(type: "TEXT", precision: 18, scale: 2, nullable: false),
                    Status = table.Column<int>(type: "INTEGER", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "TEXT", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CustomizationRequests", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CustomizationRequests_Units_UnitId",
                        column: x => x.UnitId,
                        principalTable: "Units",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "CustomizationCustomizationRequest",
                columns: table => new
                {
                    CustomizationRequestsId = table.Column<Guid>(type: "TEXT", nullable: false),
                    CustomizationsId = table.Column<Guid>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CustomizationCustomizationRequest", x => new { x.CustomizationRequestsId, x.CustomizationsId });
                    table.ForeignKey(
                        name: "FK_CustomizationCustomizationRequest_CustomizationRequests_CustomizationRequestsId",
                        column: x => x.CustomizationRequestsId,
                        principalTable: "CustomizationRequests",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CustomizationCustomizationRequest_Customizations_CustomizationsId",
                        column: x => x.CustomizationsId,
                        principalTable: "Customizations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CustomizationCustomizationRequest_CustomizationsId",
                table: "CustomizationCustomizationRequest",
                column: "CustomizationsId");

            migrationBuilder.CreateIndex(
                name: "IX_CustomizationRequests_UnitId",
                table: "CustomizationRequests",
                column: "UnitId");

            migrationBuilder.CreateIndex(
                name: "IX_Customizations_Name",
                table: "Customizations",
                column: "Name",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CustomizationCustomizationRequest");

            migrationBuilder.DropTable(
                name: "CustomizationRequests");

            migrationBuilder.DropTable(
                name: "Customizations");

            migrationBuilder.DropTable(
                name: "Units");
        }
    }
}
