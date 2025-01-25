using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SmartLoad.Migrations
{
    /// <inheritdoc />
    public partial class UpdateVehicleTypeWithKind : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AllowedRoadTypes",
                table: "VehicleTypes",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "AxleCount",
                table: "VehicleTypes",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<float>(
                name: "CouplingDevice",
                table: "VehicleTypes",
                type: "real",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.AddColumn<float>(
                name: "EmptyWeight",
                table: "VehicleTypes",
                type: "real",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.AddColumn<float>(
                name: "FrontOverhang",
                table: "VehicleTypes",
                type: "real",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.AddColumn<float>(
                name: "Height",
                table: "VehicleTypes",
                type: "real",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.AddColumn<float>(
                name: "Kingpindist",
                table: "VehicleTypes",
                type: "real",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.AddColumn<float>(
                name: "Length",
                table: "VehicleTypes",
                type: "real",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.AddColumn<float>(
                name: "MaxAxleLoad",
                table: "VehicleTypes",
                type: "real",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.AddColumn<float>(
                name: "MaxLoadCapacity",
                table: "VehicleTypes",
                type: "real",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.AddColumn<float>(
                name: "MaxVolumeCapasity",
                table: "VehicleTypes",
                type: "real",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.AddColumn<string>(
                name: "Notes",
                table: "VehicleTypes",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<float>(
                name: "OverBeyond",
                table: "VehicleTypes",
                type: "real",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.AddColumn<float>(
                name: "RearOverhang",
                table: "VehicleTypes",
                type: "real",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.AddColumn<string>(
                name: "ViewType",
                table: "VehicleTypes",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<float>(
                name: "WheelBase",
                table: "VehicleTypes",
                type: "real",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.AddColumn<float>(
                name: "Width",
                table: "VehicleTypes",
                type: "real",
                nullable: false,
                defaultValue: 0f);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AllowedRoadTypes",
                table: "VehicleTypes");

            migrationBuilder.DropColumn(
                name: "AxleCount",
                table: "VehicleTypes");

            migrationBuilder.DropColumn(
                name: "CouplingDevice",
                table: "VehicleTypes");

            migrationBuilder.DropColumn(
                name: "EmptyWeight",
                table: "VehicleTypes");

            migrationBuilder.DropColumn(
                name: "FrontOverhang",
                table: "VehicleTypes");

            migrationBuilder.DropColumn(
                name: "Height",
                table: "VehicleTypes");

            migrationBuilder.DropColumn(
                name: "Kingpindist",
                table: "VehicleTypes");

            migrationBuilder.DropColumn(
                name: "Length",
                table: "VehicleTypes");

            migrationBuilder.DropColumn(
                name: "MaxAxleLoad",
                table: "VehicleTypes");

            migrationBuilder.DropColumn(
                name: "MaxLoadCapacity",
                table: "VehicleTypes");

            migrationBuilder.DropColumn(
                name: "MaxVolumeCapasity",
                table: "VehicleTypes");

            migrationBuilder.DropColumn(
                name: "Notes",
                table: "VehicleTypes");

            migrationBuilder.DropColumn(
                name: "OverBeyond",
                table: "VehicleTypes");

            migrationBuilder.DropColumn(
                name: "RearOverhang",
                table: "VehicleTypes");

            migrationBuilder.DropColumn(
                name: "ViewType",
                table: "VehicleTypes");

            migrationBuilder.DropColumn(
                name: "WheelBase",
                table: "VehicleTypes");

            migrationBuilder.DropColumn(
                name: "Width",
                table: "VehicleTypes");
        }
    }
}
