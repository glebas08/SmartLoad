using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SmartLoad.Migrations
{
    /// <inheritdoc />
    public partial class UpdateVehicleModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Vehicles_VehicleTypes_VehicleTypeId",
                table: "Vehicles");

            migrationBuilder.RenameColumn(
                name: "MaxVolumeCapacityTrailer",
                table: "Vehicles",
                newName: "TrailerWidth");

            migrationBuilder.RenameColumn(
                name: "MaxVolumeCapacityTractor",
                table: "Vehicles",
                newName: "TrailerMaxVolumeCapacity");

            migrationBuilder.RenameColumn(
                name: "MaxLoadCapacityTrailer",
                table: "Vehicles",
                newName: "TrailerMaxLoadCapacity");

            migrationBuilder.RenameColumn(
                name: "MaxLoadCapacityTractor",
                table: "Vehicles",
                newName: "TrailerMaxAxleLoad");

            migrationBuilder.AlterColumn<int>(
                name: "VehicleTypeId",
                table: "Vehicles",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AddColumn<string>(
                name: "Notes",
                table: "Vehicles",
                type: "character varying(500)",
                maxLength: 500,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "RegistrationNumber",
                table: "Vehicles",
                type: "character varying(20)",
                maxLength: 20,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "TractorAxleCount",
                table: "Vehicles",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<float>(
                name: "TractorEmptyFrontAxleLoad",
                table: "Vehicles",
                type: "real",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.AddColumn<float>(
                name: "TractorEmptyRearAxleLoad",
                table: "Vehicles",
                type: "real",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.AddColumn<float>(
                name: "TractorEmptyWeight",
                table: "Vehicles",
                type: "real",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.AddColumn<string>(
                name: "TractorFrontAxleType",
                table: "Vehicles",
                type: "character varying(20)",
                maxLength: 20,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<float>(
                name: "TractorMaxFrontAxleLoad",
                table: "Vehicles",
                type: "real",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.AddColumn<float>(
                name: "TractorMaxLoadCapacity",
                table: "Vehicles",
                type: "real",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.AddColumn<float>(
                name: "TractorMaxRearAxleLoad",
                table: "Vehicles",
                type: "real",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.AddColumn<string>(
                name: "TractorModel",
                table: "Vehicles",
                type: "character varying(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<float>(
                name: "TractorRearAxleToKingpin",
                table: "Vehicles",
                type: "real",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.AddColumn<string>(
                name: "TractorRearAxleType",
                table: "Vehicles",
                type: "character varying(20)",
                maxLength: 20,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<float>(
                name: "TractorWheelBase",
                table: "Vehicles",
                type: "real",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.AddColumn<int>(
                name: "TrailerAxleCount",
                table: "Vehicles",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<float>(
                name: "TrailerAxleSpread",
                table: "Vehicles",
                type: "real",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.AddColumn<string>(
                name: "TrailerAxleType",
                table: "Vehicles",
                type: "character varying(20)",
                maxLength: 20,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<float>(
                name: "TrailerEmptyWeight",
                table: "Vehicles",
                type: "real",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.AddColumn<float>(
                name: "TrailerHeight",
                table: "Vehicles",
                type: "real",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.AddColumn<float>(
                name: "TrailerKingpinToAxle",
                table: "Vehicles",
                type: "real",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.AddColumn<float>(
                name: "TrailerLength",
                table: "Vehicles",
                type: "real",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.AddColumn<string>(
                name: "TrailerModel",
                table: "Vehicles",
                type: "character varying(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<string>(
                name: "Status",
                table: "LoadingSchemes",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(100)",
                oldMaxLength: 100);

            migrationBuilder.AlterColumn<string>(
                name: "Notes",
                table: "LoadingSchemes",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(500)",
                oldMaxLength: 500);

            migrationBuilder.AddColumn<int>(
                name: "RouteId",
                table: "LoadingSchemes",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_LoadingSchemes_RouteId",
                table: "LoadingSchemes",
                column: "RouteId");

            migrationBuilder.AddForeignKey(
                name: "FK_LoadingSchemes_Routes_RouteId",
                table: "LoadingSchemes",
                column: "RouteId",
                principalTable: "Routes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Vehicles_VehicleTypes_VehicleTypeId",
                table: "Vehicles",
                column: "VehicleTypeId",
                principalTable: "VehicleTypes",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LoadingSchemes_Routes_RouteId",
                table: "LoadingSchemes");

            migrationBuilder.DropForeignKey(
                name: "FK_Vehicles_VehicleTypes_VehicleTypeId",
                table: "Vehicles");

            migrationBuilder.DropIndex(
                name: "IX_LoadingSchemes_RouteId",
                table: "LoadingSchemes");

            migrationBuilder.DropColumn(
                name: "Notes",
                table: "Vehicles");

            migrationBuilder.DropColumn(
                name: "RegistrationNumber",
                table: "Vehicles");

            migrationBuilder.DropColumn(
                name: "TractorAxleCount",
                table: "Vehicles");

            migrationBuilder.DropColumn(
                name: "TractorEmptyFrontAxleLoad",
                table: "Vehicles");

            migrationBuilder.DropColumn(
                name: "TractorEmptyRearAxleLoad",
                table: "Vehicles");

            migrationBuilder.DropColumn(
                name: "TractorEmptyWeight",
                table: "Vehicles");

            migrationBuilder.DropColumn(
                name: "TractorFrontAxleType",
                table: "Vehicles");

            migrationBuilder.DropColumn(
                name: "TractorMaxFrontAxleLoad",
                table: "Vehicles");

            migrationBuilder.DropColumn(
                name: "TractorMaxLoadCapacity",
                table: "Vehicles");

            migrationBuilder.DropColumn(
                name: "TractorMaxRearAxleLoad",
                table: "Vehicles");

            migrationBuilder.DropColumn(
                name: "TractorModel",
                table: "Vehicles");

            migrationBuilder.DropColumn(
                name: "TractorRearAxleToKingpin",
                table: "Vehicles");

            migrationBuilder.DropColumn(
                name: "TractorRearAxleType",
                table: "Vehicles");

            migrationBuilder.DropColumn(
                name: "TractorWheelBase",
                table: "Vehicles");

            migrationBuilder.DropColumn(
                name: "TrailerAxleCount",
                table: "Vehicles");

            migrationBuilder.DropColumn(
                name: "TrailerAxleSpread",
                table: "Vehicles");

            migrationBuilder.DropColumn(
                name: "TrailerAxleType",
                table: "Vehicles");

            migrationBuilder.DropColumn(
                name: "TrailerEmptyWeight",
                table: "Vehicles");

            migrationBuilder.DropColumn(
                name: "TrailerHeight",
                table: "Vehicles");

            migrationBuilder.DropColumn(
                name: "TrailerKingpinToAxle",
                table: "Vehicles");

            migrationBuilder.DropColumn(
                name: "TrailerLength",
                table: "Vehicles");

            migrationBuilder.DropColumn(
                name: "TrailerModel",
                table: "Vehicles");

            migrationBuilder.DropColumn(
                name: "RouteId",
                table: "LoadingSchemes");

            migrationBuilder.RenameColumn(
                name: "TrailerWidth",
                table: "Vehicles",
                newName: "MaxVolumeCapacityTrailer");

            migrationBuilder.RenameColumn(
                name: "TrailerMaxVolumeCapacity",
                table: "Vehicles",
                newName: "MaxVolumeCapacityTractor");

            migrationBuilder.RenameColumn(
                name: "TrailerMaxLoadCapacity",
                table: "Vehicles",
                newName: "MaxLoadCapacityTrailer");

            migrationBuilder.RenameColumn(
                name: "TrailerMaxAxleLoad",
                table: "Vehicles",
                newName: "MaxLoadCapacityTractor");

            migrationBuilder.AlterColumn<int>(
                name: "VehicleTypeId",
                table: "Vehicles",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Status",
                table: "LoadingSchemes",
                type: "character varying(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "Notes",
                table: "LoadingSchemes",
                type: "character varying(500)",
                maxLength: 500,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AddForeignKey(
                name: "FK_Vehicles_VehicleTypes_VehicleTypeId",
                table: "Vehicles",
                column: "VehicleTypeId",
                principalTable: "VehicleTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
