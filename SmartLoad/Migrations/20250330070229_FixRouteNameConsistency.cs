using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SmartLoad.Migrations
{
    /// <inheritdoc />
    public partial class FixRouteNameConsistency : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LoadingSchemes_Routes_RoutId",
                table: "LoadingSchemes");

            migrationBuilder.DropColumn(
                name: "RouteId",
                table: "LoadingSchemes");

            migrationBuilder.AlterColumn<int>(
                name: "RoutId",
                table: "LoadingSchemes",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_LoadingSchemes_Routes_RoutId",
                table: "LoadingSchemes",
                column: "RoutId",
                principalTable: "Routes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LoadingSchemes_Routes_RoutId",
                table: "LoadingSchemes");

            migrationBuilder.AlterColumn<int>(
                name: "RoutId",
                table: "LoadingSchemes",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AddColumn<int>(
                name: "RouteId",
                table: "LoadingSchemes",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddForeignKey(
                name: "FK_LoadingSchemes_Routes_RoutId",
                table: "LoadingSchemes",
                column: "RoutId",
                principalTable: "Routes",
                principalColumn: "Id");
        }
    }
}
