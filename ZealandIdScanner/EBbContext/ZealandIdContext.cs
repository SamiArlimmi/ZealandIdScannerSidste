using System.Collections.Generic;
using ZealandIdScanner.Models;
using Microsoft.EntityFrameworkCore;

namespace ZealandIdScanner.EBbContext
{
        public class ZealandIdContext : DbContext
        {
        public ZealandIdContext(DbContextOptions<ZealandIdContext> options) : base(options)
        {
        }
        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            options.UseSqlServer(@"Data Source=mssql11.unoeuro.com;Initial Catalog=zealandid_dk_db_test;User ID=zealandid_dk;Password=4tn2gwfADdeRB5EGzm6b;Connect Timeout=30;Encrypt=False;Trust Server Certificate=False;Application Intent=ReadWrite;Multi Subnet Failover=False");
        }

            public DbSet<Lokaler> Lokaler { get; set; }
            public DbSet<Sensors> Sensors { get; set; }

            protected override void OnModelCreating(ModelBuilder modelBuilder)
            {

                // Configure Lokaler entity
                modelBuilder.Entity<Lokaler>()
                    .HasKey(l => l.LokaleId); // Assuming Lokaler has a property named Id

                modelBuilder.Entity<Lokaler>()
                    .Property(l => l.Navn)
                    .IsRequired()
                    .HasMaxLength(50); // Example: Set max length for the Navn property

                // Configure Sensors entity
                modelBuilder.Entity<Sensors>()
                    .HasKey(s => s.SensorId); // Assuming Sensor has a property named Id

                modelBuilder.Entity<Sensors>()
                    .Property(s => s.Navn)
                    .IsRequired()
                    .HasMaxLength(50); // Example: Set max length for the Navn property

            }
        }

}
