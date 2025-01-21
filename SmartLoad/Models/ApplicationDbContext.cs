using Microsoft.EntityFrameworkCore; 

namespace SmartLoad.Models
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<VehicleType> VehicleTypes { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<PackagingType> PackagingTypes { get; set; }
    }
}
