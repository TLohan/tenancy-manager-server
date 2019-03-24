using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace tenancy_manager_server.Entities
{
    public class AppDbContext : DbContext
    {
        public DbSet<House> Houses { get; set; }
        public DbSet<Flat> Flats { get; set; }
        public DbSet<Lease> Leases { get; set; }
        public DbSet<Tenant> Tenants { get; set; }
        public DbSet<RentReview> RentReviews { get; set; }
        public DbSet<Payment> Payments { get; set; }


        public AppDbContext(DbContextOptions<AppDbContext> dbContextOptions) : base(dbContextOptions)
        {
            Database.Migrate();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.EnableSensitiveDataLogging();
        }
    }
}
