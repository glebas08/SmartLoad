using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SmartLoad.Migrations
{
    /// <inheritdoc />
    public partial class UpdatePackagingTypeToDecimal : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Products_PackagingTypes_PackagingTypeId1",
                table: "Products");

            migrationBuilder.DropIndex(
                name: "IX_Products_PackagingTypeId1",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "PackagingTypeId1",
                table: "Products");

            migrationBuilder.AlterColumn<int>(
                name: "PackagingTypeId",
                table: "Products",
                type: "integer",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Products",
                type: "character varying(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<decimal>(
                name: "Width",
                table: "PackagingTypes",
                type: "numeric",
                nullable: false,
                oldClrType: typeof(float),
                oldType: "real");

            migrationBuilder.AlterColumn<decimal>(
                name: "Weight",
                table: "PackagingTypes",
                type: "numeric",
                nullable: false,
                oldClrType: typeof(float),
                oldType: "real");

            migrationBuilder.AlterColumn<decimal>(
                name: "Volume",
                table: "PackagingTypes",
                type: "numeric",
                nullable: false,
                oldClrType: typeof(float),
                oldType: "real");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "PackagingTypes",
                type: "character varying(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<decimal>(
                name: "Length",
                table: "PackagingTypes",
                type: "numeric",
                nullable: false,
                oldClrType: typeof(float),
                oldType: "real");

            migrationBuilder.AlterColumn<decimal>(
                name: "Height",
                table: "PackagingTypes",
                type: "numeric",
                nullable: false,
                oldClrType: typeof(float),
                oldType: "real");

            migrationBuilder.CreateIndex(
                name: "IX_Products_PackagingTypeId",
                table: "Products",
                column: "PackagingTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Products_PackagingTypes_PackagingTypeId",
                table: "Products",
                column: "PackagingTypeId",
                principalTable: "PackagingTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Products_PackagingTypes_PackagingTypeId",
                table: "Products");

            migrationBuilder.DropIndex(
                name: "IX_Products_PackagingTypeId",
                table: "Products");

            migrationBuilder.AlterColumn<string>(
                name: "PackagingTypeId",
                table: "Products",
                type: "text",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Products",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(100)",
                oldMaxLength: 100);

            migrationBuilder.AddColumn<int>(
                name: "PackagingTypeId1",
                table: "Products",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<float>(
                name: "Width",
                table: "PackagingTypes",
                type: "real",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric");

            migrationBuilder.AlterColumn<float>(
                name: "Weight",
                table: "PackagingTypes",
                type: "real",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric");

            migrationBuilder.AlterColumn<float>(
                name: "Volume",
                table: "PackagingTypes",
                type: "real",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "PackagingTypes",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(100)",
                oldMaxLength: 100);

            migrationBuilder.AlterColumn<float>(
                name: "Length",
                table: "PackagingTypes",
                type: "real",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric");

            migrationBuilder.AlterColumn<float>(
                name: "Height",
                table: "PackagingTypes",
                type: "real",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric");

            migrationBuilder.CreateIndex(
                name: "IX_Products_PackagingTypeId1",
                table: "Products",
                column: "PackagingTypeId1");

            migrationBuilder.AddForeignKey(
                name: "FK_Products_PackagingTypes_PackagingTypeId1",
                table: "Products",
                column: "PackagingTypeId1",
                principalTable: "PackagingTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
