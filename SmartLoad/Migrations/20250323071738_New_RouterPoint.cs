using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SmartLoad.Migrations
{
    /// <inheritdoc />
    public partial class New_RouterPoint : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RoutePoints_Routes_RoutId",
                table: "RoutePoints");

            migrationBuilder.DropIndex(
                name: "IX_RoutePoints_RoutId",
                table: "RoutePoints");

            migrationBuilder.DropIndex(
                name: "IX_Orders_RoutePointId1",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "RoutId",
                table: "RoutePoints");

            migrationBuilder.CreateIndex(
                name: "IX_RoutePoints_RouteId",
                table: "RoutePoints",
                column: "RouteId");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_RoutePointId1",
                table: "Orders",
                column: "RoutePointId1");

            migrationBuilder.AddForeignKey(
                name: "FK_RoutePoints_Routes_RouteId",
                table: "RoutePoints",
                column: "RouteId",
                principalTable: "Routes",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RoutePoints_Routes_RouteId",
                table: "RoutePoints");

            migrationBuilder.DropIndex(
                name: "IX_RoutePoints_RouteId",
                table: "RoutePoints");

            migrationBuilder.DropIndex(
                name: "IX_Orders_RoutePointId1",
                table: "Orders");

            migrationBuilder.AddColumn<int>(
                name: "RoutId",
                table: "RoutePoints",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_RoutePoints_RoutId",
                table: "RoutePoints",
                column: "RoutId");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_RoutePointId1",
                table: "Orders",
                column: "RoutePointId1",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_RoutePoints_Routes_RoutId",
                table: "RoutePoints",
                column: "RoutId",
                principalTable: "Routes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
