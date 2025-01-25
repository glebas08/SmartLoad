﻿// <auto-generated />
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using SmartLoad.Models;

#nullable disable

namespace SmartLoad.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20250122143712_UpdateVehicleTypeWithKind")]
    partial class UpdateVehicleTypeWithKind
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "9.0.1")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("SmartLoad.Models.PackagingType", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("PackagingTypes");
                });

            modelBuilder.Entity("SmartLoad.Models.Product", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("Products");
                });

            modelBuilder.Entity("SmartLoad.Models.VehicleType", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("AllowedRoadTypes")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("AxleCount")
                        .HasColumnType("integer");

                    b.Property<float>("CouplingDevice")
                        .HasColumnType("real");

                    b.Property<float>("EmptyWeight")
                        .HasColumnType("real");

                    b.Property<float>("FrontOverhang")
                        .HasColumnType("real");

                    b.Property<float>("Height")
                        .HasColumnType("real");

                    b.Property<float>("Kingpindist")
                        .HasColumnType("real");

                    b.Property<float>("Length")
                        .HasColumnType("real");

                    b.Property<float>("MaxAxleLoad")
                        .HasColumnType("real");

                    b.Property<float>("MaxLoadCapacity")
                        .HasColumnType("real");

                    b.Property<float>("MaxVolumeCapasity")
                        .HasColumnType("real");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Notes")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<float>("OverBeyond")
                        .HasColumnType("real");

                    b.Property<float>("RearOverhang")
                        .HasColumnType("real");

                    b.Property<string>("ViewType")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<float>("WheelBase")
                        .HasColumnType("real");

                    b.Property<float>("Width")
                        .HasColumnType("real");

                    b.HasKey("Id");

                    b.ToTable("VehicleTypes");
                });
#pragma warning restore 612, 618
        }
    }
}
