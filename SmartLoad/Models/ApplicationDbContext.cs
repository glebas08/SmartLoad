using Microsoft.EntityFrameworkCore;
using SmartLoad.Models;

namespace SmartLoad.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        //public DbSet<VehicleType> VehicleTypes { get; set; } = null!;
        public DbSet<Product> Products { get; set; } = null!;
        public DbSet<PackagingType> PackagingTypes { get; set; } = null!;
        public DbSet<Order> Orders { get; set; } = null!;
        public DbSet<OrderProduct> OrderProducts { get; set; } = null!;
        public DbSet<Rout> Routes { get; set; } = null!;
        public DbSet<RoutePoint> RoutePoints { get; set; } = null!;
        public DbSet<LoadingScheme> LoadingSchemes { get; set; } = null!;
        public DbSet<LoadingProduct> LoadingProducts { get; set; } = null!;
        public DbSet<Vehicle> Vehicles { get; set; } = null!;
        public DbSet<Distributor> Distributors { get; set; } = null!;
        public DbSet<RoutePointMapping> RoutePointMappings { get; set; } = null!;
        public DbSet<LoadingSchemeItem> LoadingSchemeItems { get; set; } = null!;

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

            modelBuilder.Entity<Order>()
                .HasKey(o => o.Id);

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

            // Связь один-к-одному между Order и RoutePoint
            modelBuilder.Entity<Order>()
                 .HasOne(o => o.RoutePoint)
                 .WithMany(rp => rp.Orders) // Добавляем навигационное свойство
                 .HasForeignKey(o => o.RoutePointId)
                 .OnDelete(DeleteBehavior.Restrict);

            // Связь между Order и Distributor
            modelBuilder.Entity<Distributor>()
                .HasMany(d => d.Orders)
                .WithOne(o => o.Distributor)
                .HasForeignKey(o => o.DistributorId)
                .OnDelete(DeleteBehavior.Cascade);

            // Связь между LoadingScheme и Vehicle
            modelBuilder.Entity<LoadingScheme>()
                .HasOne(ls => ls.Vehicle)
                .WithMany(v => v.LoadingSchemes)
                .HasForeignKey(ls => ls.VehicleId)
                .OnDelete(DeleteBehavior.Cascade);

            // Связь между LoadingScheme и VehicleType
            //modelBuilder.Entity<LoadingScheme>()
            //    .HasOne(ls => ls.VehicleType)
            //    .WithMany(vt => vt.LoadingSchemes)
            //    .HasForeignKey(ls => ls.VehicleTypeId)
            //    .OnDelete(DeleteBehavior.Cascade);

            // Связь между LoadingProduct и LoadingScheme
            //modelBuilder.Entity<LoadingProduct>()
            //    .HasOne(lp => lp.LoadingScheme)
            //    .WithMany(ls => ls.LoadingProducts)
            //    .HasForeignKey(lp => lp.LoadingSchemeId)
            //    .OnDelete(DeleteBehavior.Cascade);

            // Связь между LoadingProduct и Product
            modelBuilder.Entity<LoadingProduct>()
                .HasOne(lp => lp.Product)
                .WithMany(p => p.LoadingProducts)
                .HasForeignKey(lp => lp.ProductId)
                .OnDelete(DeleteBehavior.Cascade);

            //// Связь между Vehicle и VehicleType
            //modelBuilder.Entity<Vehicle>()
            //    .HasOne(v => v.VehicleType)
            //    .WithMany(vt => vt.Vehicles)
            //    .HasForeignKey(v => v.VehicleTypeId)
            //    .OnDelete(DeleteBehavior.Cascade);

            // Связь между Vehicle и LoadingScheme
            modelBuilder.Entity<Vehicle>()
                .HasMany(v => v.LoadingSchemes)
                .WithOne(ls => ls.Vehicle)
                .HasForeignKey(ls => ls.VehicleId)
                .OnDelete(DeleteBehavior.Cascade);

            //// Связь между RoutePoint и Rout (старая связь, можно постепенно удалить)
            //modelBuilder.Entity<RoutePoint>()
            //    .HasOne(rp => rp.Rout)
            //    .WithMany(r => r.RoutePoints)
            //    .HasForeignKey(rp => rp.RouteId)
            //    .OnDelete(DeleteBehavior.SetNull); // Устанавливаем поведение при удалении на SetNull

            // Настройте связь через промежуточную таблицу RoutePointMapping
            modelBuilder.Entity<RoutePointMapping>()
                .HasKey(rpm => rpm.Id); // Первичный ключ

            modelBuilder.Entity<RoutePointMapping>()
                .HasOne(rpm => rpm.Route)
                .WithMany(r => r.RoutePointMappings)
                .HasForeignKey(rpm => rpm.RouteId)
                .OnDelete(DeleteBehavior.Cascade); // При удалении маршрута удаляем все связи

            modelBuilder.Entity<LoadingSchemeItem>()
                .HasOne(lsi => lsi.LoadingScheme)
                .WithMany(ls => ls.LoadingSchemeItems)
                .HasForeignKey(lsi => lsi.LoadingSchemeId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<LoadingSchemeItem>()
                .HasOne(lsi => lsi.Product)
                .WithMany()
                .HasForeignKey(lsi => lsi.ProductId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<RoutePointMapping>()
                .HasOne(rpm => rpm.RoutePoint)
                .WithMany(rp => rp.RoutePointMappings)
                .HasForeignKey(rpm => rpm.RoutePointId)
                .OnDelete(DeleteBehavior.Cascade); // При удалении точки маршрута удаляем все связи
        }
    }
}