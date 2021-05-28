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
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
            this.ChangeTracker.LazyLoadingEnabled = false;
        }
        public DbSet<Country> Countries { get; set; }
        public DbSet<FromDate> FromDates { get; set; }
        public DbSet<ToDate> ToDates { get; set; }
        public DbSet<HolidayType> HolidayTypes { get; set; }
        public DbSet<Country_HolidayType> Country_HolidayTypes { get; set; }
        public DbSet<Region> Regions { get; set; }
        public DbSet<Holiday> Holidays { get; set; }
        public DbSet<HolidayDate> HolidayDates { get; set; }
        public DbSet<HolidayName> HolidayNames { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Country>()
                .HasMany(a => a.Regions)
                .WithOne(a => a.Country)
                .HasForeignKey(a => a.CountryCode);

            modelBuilder.Entity<Country>()
                .HasOne(a => a.FromDate)
                .WithOne(a => a.Country)
                .HasForeignKey<FromDate>(a => a.CountryCode);

            modelBuilder.Entity<Country>()
              .HasOne(a => a.ToDate)
              .WithOne(a => a.Country)
              .HasForeignKey<ToDate>(a => a.CountryCode);

            modelBuilder.Entity<Country>()
                .HasMany(a => a.Holiday)
                .WithOne(a => a.Country)
                .HasForeignKey(a => a.CountryCode);

            modelBuilder.Entity<Country_HolidayType>()
                .HasOne(c => c.Country)
                .WithMany(ch => ch.Country_HolidayTypes)
                .HasForeignKey(c => c.CountryCode);

            modelBuilder.Entity<Country_HolidayType>()
                .HasOne(c => c.HolidayType)
                .WithMany(ch => ch.Country_HolidayTypes)
                .HasForeignKey(c => c.HolidayTypeId);
        }
    }
}
