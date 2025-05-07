using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace SmartLoad.Migrations
{
    /// <inheritdoc />
    public partial class UpdateLoadingSchemeModels : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LoadingSchemes_Routes_RouteId",
                table: "LoadingSchemes");

            migrationBuilder.DropForeignKey(
                name: "FK_LoadingSchemes_VehicleTypes_VehicleTypeId",
                table: "LoadingSchemes");

            migrationBuilder.DropTable(
                name: "VehicleTypes");

            migrationBuilder.DropIndex(
                name: "IX_LoadingSchemes_RouteId",
                table: "LoadingSchemes");

            migrationBuilder.DropIndex(
                name: "IX_LoadingSchemes_VehicleTypeId",
                table: "LoadingSchemes");

            migrationBuilder.DropColumn(
                name: "Notes",
                table: "LoadingSchemes");

            migrationBuilder.DropColumn(
                name: "VehicleTypeId",
                table: "LoadingSchemes");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedDate",
                table: "LoadingSchemes",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "RoutId",
                table: "LoadingSchemes",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "LoadingSchemeItems",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    LoadingSchemeId = table.Column<int>(type: "integer", nullable: false),
                    ProductId = table.Column<int>(type: "integer", nullable: false),
                    PositionX = table.Column<float>(type: "real", nullable: false),
                    PositionY = table.Column<float>(type: "real", nullable: false),
                    PositionZ = table.Column<float>(type: "real", nullable: false),
                    Destination = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LoadingSchemeItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LoadingSchemeItems_LoadingSchemes_LoadingSchemeId",
                        column: x => x.LoadingSchemeId,
                        principalTable: "LoadingSchemes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_LoadingSchemeItems_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_LoadingSchemes_RoutId",
                table: "LoadingSchemes",
                column: "RoutId");

            migrationBuilder.CreateIndex(
                name: "IX_LoadingSchemeItems_LoadingSchemeId",
                table: "LoadingSchemeItems",
                column: "LoadingSchemeId");

            migrationBuilder.CreateIndex(
                name: "IX_LoadingSchemeItems_ProductId",
                table: "LoadingSchemeItems",
                column: "ProductId");

            migrationBuilder.AddForeignKey(
                name: "FK_LoadingSchemes_Routes_RoutId",
                table: "LoadingSchemes",
                column: "RoutId",
                principalTable: "Routes",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LoadingSchemes_Routes_RoutId",
                table: "LoadingSchemes");

            migrationBuilder.DropTable(
                name: "LoadingSchemeItems");

            migrationBuilder.DropIndex(
                name: "IX_LoadingSchemes_RoutId",
                table: "LoadingSchemes");

            migrationBuilder.DropColumn(
                name: "CreatedDate",
                table: "LoadingSchemes");

            migrationBuilder.DropColumn(
                name: "RoutId",
                table: "LoadingSchemes");

            migrationBuilder.AddColumn<string>(
                name: "Notes",
                table: "LoadingSchemes",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "VehicleTypeId",
                table: "LoadingSchemes",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "VehicleTypes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    AllowedRoadTypes = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    AxleCount = table.Column<int>(type: "integer", nullable: false),
                    CouplingDevice = table.Column<float>(type: "real", nullable: false),
                    EmptyWeight = table.Column<float>(type: "real", nullable: false),
                    FrontOverhang = table.Column<float>(type: "real", nullable: false),
                    Height = table.Column<float>(type: "real", nullable: false),
                    Kingpindist = table.Column<float>(type: "real", nullable: false),
                    Length = table.Column<float>(type: "real", nullable: false),
                    MaxAxleLoad = table.Column<float>(type: "real", nullable: false),
                    MaxLoadCapacity = table.Column<float>(type: "real", nullable: false),
                    MaxVolumeCapacity = table.Column<float>(type: "real", nullable: false),
                    Name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Notes = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    OverBeyond = table.Column<float>(type: "real", nullable: false),
                    RearOverhang = table.Column<float>(type: "real", nullable: false),
                    ViewType = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    WheelBase = table.Column<float>(type: "real", nullable: false),
                    Width = table.Column<float>(type: "real", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VehicleTypes", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_LoadingSchemes_RouteId",
                table: "LoadingSchemes",
                column: "RouteId");

            migrationBuilder.CreateIndex(
                name: "IX_LoadingSchemes_VehicleTypeId",
                table: "LoadingSchemes",
                column: "VehicleTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_LoadingSchemes_Routes_RouteId",
                table: "LoadingSchemes",
                column: "RouteId",
                principalTable: "Routes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_LoadingSchemes_VehicleTypes_VehicleTypeId",
                table: "LoadingSchemes",
                column: "VehicleTypeId",
                principalTable: "VehicleTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
