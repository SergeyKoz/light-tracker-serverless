﻿// <auto-generated />
using LightTrackerServerless.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace LightTrackerServerless.Database.Migrations
{
    [DbContext(typeof(LightTrackerContext))]
    [Migration("20240630120528_Initial")]
    partial class Initial
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.6")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("LightTrackerServerless.Database.Models.Device", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("DeviceUniqueIdentifier")
                        .HasColumnType("nvarchar(450)");

                    b.Property<float>("BatteryLevel")
                        .HasColumnType("real");

                    b.Property<int>("BatteryStatus")
                        .HasColumnType("int");

                    b.Property<long>("CreatedAt")
                        .HasColumnType("bigint");

                    b.Property<string>("DeviceName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("NetworkReachability")
                        .HasColumnType("int");

                    b.Property<long>("UpdatedAt")
                        .HasColumnType("bigint");

                    b.HasKey("UserId", "DeviceUniqueIdentifier");

                    b.ToTable("Devices");
                });
#pragma warning restore 612, 618
        }
    }
}
