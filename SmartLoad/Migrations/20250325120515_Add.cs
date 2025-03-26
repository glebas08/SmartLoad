using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace SmartLoad.Migrations
{
    /// <inheritdoc />
    public partial class Add : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Orders_RoutePoints_RoutePointId1",
                table: "Orders");

            migrationBuilder.DropIndex(
                name: "IX_Orders_RoutePointId1",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "RoutePointId1",
                table: "Orders");

            migrationBuilder.CreateTable(
                name: "RoutePointMappings",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    RouteId = table.Column<int>(type: "integer", nullable: false),
                    RoutePointId = table.Column<int>(type: "integer", nullable: false),
                    OrderInRoute = table.Column<int>(type: "integer", nullable: false),
                    EstimatedArrivalTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Notes = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RoutePointMappings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RoutePointMappings_RoutePoints_RoutePointId",
                        column: x => x.RoutePointId,
                        principalTable: "RoutePoints",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_RoutePointMappings_Routes_RouteId",
                        column: x => x.RouteId,
                        principalTable: "Routes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_RoutePointMappings_RouteId",
                table: "RoutePointMappings",
                column: "RouteId");

            migrationBuilder.CreateIndex(
                name: "IX_RoutePointMappings_RoutePointId",
                table: "RoutePointMappings",
                column: "RoutePointId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RoutePointMappings");

            migrationBuilder.AddColumn<int>(
                name: "RoutePointId1",
                table: "Orders",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Orders_RoutePointId1",
                table: "Orders",
                column: "RoutePointId1");

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_RoutePoints_RoutePointId1",
                table: "Orders",
                column: "RoutePointId1",
                principalTable: "RoutePoints",
                principalColumn: "Id");
        }
    }
}
