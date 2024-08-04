using LightTrackerServerless.Database.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LightTrackerServerless.Database
{
    public class LightTrackerContext : DbContext
    {
        public virtual DbSet<Device> Devices { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (optionsBuilder.IsConfigured)
            {
                return;
            }

            var connectionString = Environment.GetEnvironmentVariable("DB_CONNECTION") ?? "";

            optionsBuilder.UseSqlServer(connectionString);

            base.OnConfiguring(optionsBuilder);
        }

        //protected override void OnModelCreating(ModelBuilder modelBuilder)
        //{
        //     modelBuilder.Entity<Device>().HasKey(m => new { m.UserId, m.DeviceUniqueIdentifier });
        //}
    }
}
