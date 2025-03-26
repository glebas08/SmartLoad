using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SmartLoad.Migrations
{
    /// <inheritdoc />
    public partial class AllowNullForRoutePointMappingNotes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RoutePointMappings_RoutePoints_RoutePointId",
                table: "RoutePointMappings");

            migrationBuilder.AddForeignKey(
                name: "FK_RoutePointMappings_RoutePoints_RoutePointId",
                table: "RoutePointMappings",
                column: "RoutePointId",
                principalTable: "RoutePoints",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RoutePointMappings_RoutePoints_RoutePointId",
                table: "RoutePointMappings");

            migrationBuilder.AddForeignKey(
                name: "FK_RoutePointMappings_RoutePoints_RoutePointId",
                table: "RoutePointMappings",
                column: "RoutePointId",
                principalTable: "RoutePoints",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
