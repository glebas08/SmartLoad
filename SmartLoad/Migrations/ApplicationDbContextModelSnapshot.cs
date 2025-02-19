﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using SmartLoad.Data;

#nullable disable

namespace SmartLoad.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    partial class ApplicationDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "9.0.1")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("SmartLoad.Models.LoadingProduct", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<int>("LoadingSchemeId")
                        .HasColumnType("integer");

                    b.Property<int>("OrderId")
                        .HasColumnType("integer");

                    b.Property<int>("PackagingTypeId")
                        .HasColumnType("integer");

                    b.Property<float>("PositionX")
                        .HasColumnType("real");

                    b.Property<float>("PositionY")
                        .HasColumnType("real");

                    b.Property<float>("PositionZ")
                        .HasColumnType("real");

                    b.Property<int>("ProductId")
                        .HasColumnType("integer");

                    b.Property<int>("Quantity")
                        .HasColumnType("integer");

                    b.Property<int>("RoutePointId")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("LoadingSchemeId");

                    b.HasIndex("OrderId");

                    b.HasIndex("PackagingTypeId");

                    b.HasIndex("ProductId");

                    b.HasIndex("RoutePointId");

                    b.ToTable("LoadingProducts");
                });

            modelBuilder.Entity("SmartLoad.Models.LoadingScheme", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("LoadingDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Notes")
                        .IsRequired()
                        .HasMaxLength(500)
                        .HasColumnType("character varying(500)");

                    b.Property<int>("RouteId")
                        .HasColumnType("integer");

                    b.Property<string>("Status")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)");

                    b.Property<int>("VehicleId")
                        .HasColumnType("integer");

                    b.Property<int>("VehicleTypeId")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("RouteId");

                    b.HasIndex("VehicleId");

                    b.HasIndex("VehicleTypeId");

                    b.ToTable("LoadingSchemes");
                });

            modelBuilder.Entity("SmartLoad.Models.Order", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Notes")
                        .IsRequired()
                        .HasMaxLength(500)
                        .HasColumnType("character varying(500)");

                    b.Property<DateTime>("OrderDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<int>("RouteId")
                        .HasColumnType("integer");

                    b.Property<string>("Status")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)");

                    b.HasKey("Id");

                    b.HasIndex("RouteId");

                    b.ToTable("Orders");
                });

            modelBuilder.Entity("SmartLoad.Models.OrderProduct", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<int>("OrderId")
                        .HasColumnType("integer");

                    b.Property<int>("ProductId")
                        .HasColumnType("integer");

                    b.Property<int>("Quantity")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("OrderId");

                    b.HasIndex("ProductId");

                    b.ToTable("OrderProducts");
                });

            modelBuilder.Entity("SmartLoad.Models.OrderRoutePoint", b =>
                {
                    b.Property<int>("OrderId")
                        .HasColumnType("integer");

                    b.Property<int>("RoutePointId")
                        .HasColumnType("integer");

                    b.Property<int>("Id")
                        .HasColumnType("integer");

                    b.HasKey("OrderId", "RoutePointId");

                    b.HasIndex("RoutePointId");

                    b.ToTable("OrderRoutePoints");
                });

            modelBuilder.Entity("SmartLoad.Models.PackagingType", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<float>("Height")
                        .HasColumnType("real");

                    b.Property<float>("Length")
                        .HasColumnType("real");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)");

                    b.Property<int>("ProductId")
                        .HasColumnType("integer");

                    b.Property<float>("Weight")
                        .HasColumnType("real");

                    b.Property<float>("Width")
                        .HasColumnType("real");

                    b.HasKey("Id");

                    b.HasIndex("ProductId");

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
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)");

                    b.HasKey("Id");

                    b.ToTable("Products");
                });

            modelBuilder.Entity("SmartLoad.Models.Rout", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("ArrivalDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<DateTime>("DepartureDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)");

                    b.Property<string>("Notes")
                        .IsRequired()
                        .HasMaxLength(500)
                        .HasColumnType("character varying(500)");

                    b.Property<DateTime>("RouteDate")
                        .HasColumnType("timestamp with time zone");

                    b.HasKey("Id");

                    b.ToTable("Routes");
                });

            modelBuilder.Entity("SmartLoad.Models.RoutePoint", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)");

                    b.Property<int>("RouteId")
                        .HasColumnType("integer");

                    b.Property<DateTime>("UnloadingDate")
                        .HasColumnType("timestamp with time zone");

                    b.HasKey("Id");

                    b.HasIndex("RouteId");

                    b.ToTable("RoutePoints");
                });

            modelBuilder.Entity("SmartLoad.Models.Vehicle", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<float>("MaxLoadCapacityTractor")
                        .HasColumnType("real");

                    b.Property<float>("MaxLoadCapacityTrailer")
                        .HasColumnType("real");

                    b.Property<float>("MaxVolumeCapacityTractor")
                        .HasColumnType("real");

                    b.Property<float>("MaxVolumeCapacityTrailer")
                        .HasColumnType("real");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)");

                    b.Property<int>("VehicleTypeId")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("VehicleTypeId");

                    b.ToTable("Vehicles");
                });

            modelBuilder.Entity("SmartLoad.Models.VehicleType", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("AllowedRoadTypes")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("character varying(255)");

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

                    b.Property<float>("MaxVolumeCapacity")
                        .HasColumnType("real");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)");

                    b.Property<string>("Notes")
                        .IsRequired()
                        .HasMaxLength(500)
                        .HasColumnType("character varying(500)");

                    b.Property<float>("OverBeyond")
                        .HasColumnType("real");

                    b.Property<float>("RearOverhang")
                        .HasColumnType("real");

                    b.Property<string>("ViewType")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)");

                    b.Property<float>("WheelBase")
                        .HasColumnType("real");

                    b.Property<float>("Width")
                        .HasColumnType("real");

                    b.HasKey("Id");

                    b.ToTable("VehicleTypes");
                });

            modelBuilder.Entity("SmartLoad.Models.LoadingProduct", b =>
                {
                    b.HasOne("SmartLoad.Models.LoadingScheme", "LoadingScheme")
                        .WithMany("LoadingProducts")
                        .HasForeignKey("LoadingSchemeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("SmartLoad.Models.Order", "Order")
                        .WithMany("LoadingProducts")
                        .HasForeignKey("OrderId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("SmartLoad.Models.PackagingType", "PackagingType")
                        .WithMany()
                        .HasForeignKey("PackagingTypeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("SmartLoad.Models.Product", "Product")
                        .WithMany("LoadingProducts")
                        .HasForeignKey("ProductId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("SmartLoad.Models.RoutePoint", "RoutePoint")
                        .WithMany("LoadingProducts")
                        .HasForeignKey("RoutePointId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("LoadingScheme");

                    b.Navigation("Order");

                    b.Navigation("PackagingType");

                    b.Navigation("Product");

                    b.Navigation("RoutePoint");
                });

            modelBuilder.Entity("SmartLoad.Models.LoadingScheme", b =>
                {
                    b.HasOne("SmartLoad.Models.Rout", "Rout")
                        .WithMany("LoadingSchemes")
                        .HasForeignKey("RouteId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("SmartLoad.Models.Vehicle", "Vehicle")
                        .WithMany("LoadingSchemes")
                        .HasForeignKey("VehicleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("SmartLoad.Models.VehicleType", "VehicleType")
                        .WithMany("LoadingSchemes")
                        .HasForeignKey("VehicleTypeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Rout");

                    b.Navigation("Vehicle");

                    b.Navigation("VehicleType");
                });

            modelBuilder.Entity("SmartLoad.Models.Order", b =>
                {
                    b.HasOne("SmartLoad.Models.Rout", "Rout")
                        .WithMany("Orders")
                        .HasForeignKey("RouteId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Rout");
                });

            modelBuilder.Entity("SmartLoad.Models.OrderProduct", b =>
                {
                    b.HasOne("SmartLoad.Models.Order", "Order")
                        .WithMany("OrderProducts")
                        .HasForeignKey("OrderId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("SmartLoad.Models.Product", "Product")
                        .WithMany()
                        .HasForeignKey("ProductId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Order");

                    b.Navigation("Product");
                });

            modelBuilder.Entity("SmartLoad.Models.OrderRoutePoint", b =>
                {
                    b.HasOne("SmartLoad.Models.Order", "Order")
                        .WithMany("OrderRoutePoints")
                        .HasForeignKey("OrderId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("SmartLoad.Models.RoutePoint", "RoutePoint")
                        .WithMany("OrderRoutePoints")
                        .HasForeignKey("RoutePointId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Order");

                    b.Navigation("RoutePoint");
                });

            modelBuilder.Entity("SmartLoad.Models.PackagingType", b =>
                {
                    b.HasOne("SmartLoad.Models.Product", "Product")
                        .WithMany("PackagingTypes")
                        .HasForeignKey("ProductId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Product");
                });

            modelBuilder.Entity("SmartLoad.Models.RoutePoint", b =>
                {
                    b.HasOne("SmartLoad.Models.Rout", "Rout")
                        .WithMany("RoutePoints")
                        .HasForeignKey("RouteId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Rout");
                });

            modelBuilder.Entity("SmartLoad.Models.Vehicle", b =>
                {
                    b.HasOne("SmartLoad.Models.VehicleType", "VehicleType")
                        .WithMany("Vehicles")
                        .HasForeignKey("VehicleTypeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("VehicleType");
                });

            modelBuilder.Entity("SmartLoad.Models.LoadingScheme", b =>
                {
                    b.Navigation("LoadingProducts");
                });

            modelBuilder.Entity("SmartLoad.Models.Order", b =>
                {
                    b.Navigation("LoadingProducts");

                    b.Navigation("OrderProducts");

                    b.Navigation("OrderRoutePoints");
                });

            modelBuilder.Entity("SmartLoad.Models.Product", b =>
                {
                    b.Navigation("LoadingProducts");

                    b.Navigation("PackagingTypes");
                });

            modelBuilder.Entity("SmartLoad.Models.Rout", b =>
                {
                    b.Navigation("LoadingSchemes");

                    b.Navigation("Orders");

                    b.Navigation("RoutePoints");
                });

            modelBuilder.Entity("SmartLoad.Models.RoutePoint", b =>
                {
                    b.Navigation("LoadingProducts");

                    b.Navigation("OrderRoutePoints");
                });

            modelBuilder.Entity("SmartLoad.Models.Vehicle", b =>
                {
                    b.Navigation("LoadingSchemes");
                });

            modelBuilder.Entity("SmartLoad.Models.VehicleType", b =>
                {
                    b.Navigation("LoadingSchemes");

                    b.Navigation("Vehicles");
                });
#pragma warning restore 612, 618
        }
    }
}
