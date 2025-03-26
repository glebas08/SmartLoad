using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SmartLoad.Migrations
{
    /// <inheritdoc />
    public partial class RemoveDirectRoutePointRelation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RoutePoints_Routes_RouteId",
                table: "RoutePoints");

            migrationBuilder.DropIndex(
                name: "IX_RoutePoints_RouteId",
                table: "RoutePoints");

            migrationBuilder.DropColumn(
                name: "RouteId",
                table: "RoutePoints");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "RouteId",
                table: "RoutePoints",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_RoutePoints_RouteId",
                table: "RoutePoints",
                column: "RouteId");

            migrationBuilder.AddForeignKey(
                name: "FK_RoutePoints_Routes_RouteId",
                table: "RoutePoints",
                column: "RouteId",
                principalTable: "Routes",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }
    }
}
