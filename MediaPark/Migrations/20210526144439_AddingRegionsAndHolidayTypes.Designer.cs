﻿// <auto-generated />
using System;
using MediaPark.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace MediaPark.Migrations
{
    [DbContext(typeof(AppDbContext))]
    [Migration("20210526144439_AddingRegionsAndHolidayTypes")]
    partial class AddingRegionsAndHolidayTypes
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("ProductVersion", "5.0.6")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("MediaPark.Entities.Country", b =>
                {
                    b.Property<string>("CountryCode")
                        .HasColumnType("nvarchar(450)");

                    b.Property<int?>("FromDateId")
                        .HasColumnType("int");

                    b.Property<string>("FullName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("ToDateId")
                        .HasColumnType("int");

                    b.HasKey("CountryCode");

                    b.HasIndex("FromDateId");

                    b.HasIndex("ToDateId");

                    b.ToTable("Countries");
                });

            modelBuilder.Entity("MediaPark.Entities.FromDate", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("Day")
                        .HasColumnType("int");

                    b.Property<int>("Month")
                        .HasColumnType("int");

                    b.Property<int>("Year")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("FromDates");
                });

            modelBuilder.Entity("MediaPark.Entities.HolidayType", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("CountryCode")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("CountryCode");

                    b.ToTable("HolidayTypes");
                });

            modelBuilder.Entity("MediaPark.Entities.Region", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("CountryCode")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("CountryCode");

                    b.ToTable("Regions");
                });

            modelBuilder.Entity("MediaPark.Entities.ToDate", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("Day")
                        .HasColumnType("int");

                    b.Property<int>("Month")
                        .HasColumnType("int");

                    b.Property<int>("Year")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("ToDates");
                });

            modelBuilder.Entity("MediaPark.Entities.Country", b =>
                {
                    b.HasOne("MediaPark.Entities.FromDate", "FromDate")
                        .WithMany()
                        .HasForeignKey("FromDateId");

                    b.HasOne("MediaPark.Entities.ToDate", "ToDate")
                        .WithMany()
                        .HasForeignKey("ToDateId");

                    b.Navigation("FromDate");

                    b.Navigation("ToDate");
                });

            modelBuilder.Entity("MediaPark.Entities.HolidayType", b =>
                {
                    b.HasOne("MediaPark.Entities.Country", null)
                        .WithMany("HolidayTypes")
                        .HasForeignKey("CountryCode");
                });

            modelBuilder.Entity("MediaPark.Entities.Region", b =>
                {
                    b.HasOne("MediaPark.Entities.Country", null)
                        .WithMany("Regions")
                        .HasForeignKey("CountryCode");
                });

            modelBuilder.Entity("MediaPark.Entities.Country", b =>
                {
                    b.Navigation("HolidayTypes");

                    b.Navigation("Regions");
                });
#pragma warning restore 612, 618
        }
    }
}