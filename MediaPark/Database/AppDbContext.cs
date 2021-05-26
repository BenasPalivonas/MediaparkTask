using MediaPark.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MediaPark.Database
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) {
        }
        public DbSet<Country> Countries {get; set;}
        public DbSet<FromDate> FromDates { get; set; }
        public DbSet<ToDate> ToDates { get; set; }
        public DbSet<HolidayType> HolidayTypes { get; set; }
        public DbSet<Region> Regions { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Country>()
                .HasOne(a => a.FromDate)
                .WithOne(a => a.Country)
                .HasForeignKey<FromDate>(a => a.CountryCode);

            modelBuilder.Entity<Country>()
              .HasOne(a => a.ToDate)
              .WithOne(a => a.Country)
              .HasForeignKey<ToDate>(a => a.CountryCode);

        }
    }
}
