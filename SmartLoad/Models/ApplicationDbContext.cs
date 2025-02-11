using Microsoft.EntityFrameworkCore;
using SmartLoad.Models;

namespace SmartLoad.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<VehicleType> VehicleTypes { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<PackagingType> PackagingTypes { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderProduct> OrderProducts { get; set; }
        public DbSet<SmartLoad.Models.Rout> Routes { get; set; } // Переименование в SmartLoad.Models.Route
        public DbSet<RoutePoint> RoutePoints { get; set; }
        public DbSet<LoadingScheme> LoadingSchemes { get; set; }
        public DbSet<LoadingProduct> LoadingProducts { get; set; }
        public DbSet<Vehicle> Vehicles { get; set; }
        public DbSet<OrderRoutePoint> OrderRoutePoints { get; set; } // Добавляем новую модель

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Связь между Product и PackagingType
            modelBuilder.Entity<Product>()
                .HasMany(p => p.PackagingTypes)
                .WithOne(pt => pt.Product)
                .HasForeignKey(pt => pt.ProductId)
                .OnDelete(DeleteBehavior.Cascade);

            // Связь между Product и LoadingProduct
            modelBuilder.Entity<Product>()
                .HasMany(p => p.LoadingProducts)
                .WithOne(lp => lp.Product)
                .HasForeignKey(lp => lp.ProductId)
                .OnDelete(DeleteBehavior.Cascade);

            // Связь между Order и OrderProduct
            modelBuilder.Entity<Order>()
                .HasMany(o => o.OrderProducts)
                .WithOne(op => op.Order)
                .HasForeignKey(op => op.OrderId)
                .OnDelete(DeleteBehavior.Cascade);

            // Связь между Order и LoadingProduct
            modelBuilder.Entity<Order>()
                .HasMany(o => o.LoadingProducts)
                .WithOne(lp => lp.Order)
                .HasForeignKey(lp => lp.OrderId)
                .OnDelete(DeleteBehavior.Cascade);

            // Связь между Order и RoutePoint через OrderRoutePoint
            modelBuilder.Entity<Order>()
                .HasMany(o => o.OrderRoutePoints)
                .WithOne(orp => orp.Order)
                .HasForeignKey(orp => orp.OrderId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<RoutePoint>()
                .HasMany(rp => rp.OrderRoutePoints)
                .WithOne(orp => orp.RoutePoint)
                .HasForeignKey(orp => orp.RoutePointId)
                .OnDelete(DeleteBehavior.Cascade);

            // Связь между Route и RoutePoint
            modelBuilder.Entity<SmartLoad.Models.Rout>()
                .HasMany(r => r.RoutePoints)
                .WithOne(rp => rp.Rout)
                .HasForeignKey(rp => rp.RouteId)
                .OnDelete(DeleteBehavior.Cascade);

            // Связь между Route и LoadingScheme
            modelBuilder.Entity<SmartLoad.Models.Rout>()
                .HasMany(r => r.LoadingSchemes)
                .WithOne(ls => ls.Rout)
                .HasForeignKey(ls => ls.RouteId)
                .OnDelete(DeleteBehavior.Cascade);

            // Связь между LoadingScheme и Vehicle
            modelBuilder.Entity<LoadingScheme>()
                .HasOne(ls => ls.Vehicle)
                .WithMany(v => v.LoadingSchemes)
                .HasForeignKey(ls => ls.VehicleId)
                .OnDelete(DeleteBehavior.Cascade);

            // Связь между LoadingScheme и VehicleType
            modelBuilder.Entity<LoadingScheme>()
                .HasOne(ls => ls.VehicleType)
                .WithMany(vt => vt.LoadingSchemes)
                .HasForeignKey(ls => ls.VehicleTypeId)
                .OnDelete(DeleteBehavior.Cascade);

            // Связь между LoadingProduct и LoadingScheme
            modelBuilder.Entity<LoadingProduct>()
                .HasOne(lp => lp.LoadingScheme)
                .WithMany(ls => ls.LoadingProducts)
                .HasForeignKey(lp => lp.LoadingSchemeId)
                .OnDelete(DeleteBehavior.Cascade);

            // Связь между LoadingProduct и Product
            modelBuilder.Entity<LoadingProduct>()
                .HasOne(lp => lp.Product)
                .WithMany(p => p.LoadingProducts)
                .HasForeignKey(lp => lp.ProductId)
                .OnDelete(DeleteBehavior.Cascade);

            // Связь между Vehicle и VehicleType
            modelBuilder.Entity<Vehicle>()
                .HasOne(v => v.VehicleType)
                .WithMany(vt => vt.Vehicles)
                .HasForeignKey(v => v.VehicleTypeId)
                .OnDelete(DeleteBehavior.Cascade);

            // Связь между Vehicle и LoadingScheme
            modelBuilder.Entity<Vehicle>()
                .HasMany(v => v.LoadingSchemes)
                .WithOne(ls => ls.Vehicle)
                .HasForeignKey(ls => ls.VehicleId)
                .OnDelete(DeleteBehavior.Cascade);

            // Настройка составного ключа для OrderRoutePoint
            modelBuilder.Entity<OrderRoutePoint>()
                .HasKey(orp => new { orp.OrderId, orp.RoutePointId });

            modelBuilder.Entity<OrderRoutePoint>()
                .HasOne(orp => orp.Order)
                .WithMany(o => o.OrderRoutePoints)
                .HasForeignKey(orp => orp.OrderId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<OrderRoutePoint>()
                .HasOne(orp => orp.RoutePoint)
                .WithMany(rp => rp.OrderRoutePoints)
                .HasForeignKey(orp => orp.RoutePointId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}