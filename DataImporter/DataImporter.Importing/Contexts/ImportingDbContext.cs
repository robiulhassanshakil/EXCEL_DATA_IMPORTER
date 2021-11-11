using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataImporter.Importing.Entities;
using DataImporter.Membership.Entities;
using Microsoft.EntityFrameworkCore;

namespace DataImporter.Importing.Contexts
{
    public class ImportingDbContext : DbContext, IImportingDbContext
    {
        private readonly string _connectionString;
        private readonly string _migrationAssemblyName;

        public ImportingDbContext(string connectionString, string migrationAssemblyName)
        {
            _connectionString = connectionString;
            _migrationAssemblyName = migrationAssemblyName;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder dbContextOptionsBuilder)
        {
            if (!dbContextOptionsBuilder.IsConfigured)
            {
                dbContextOptionsBuilder.UseSqlServer(
                    _connectionString,
                    m => m.MigrationsAssembly(_migrationAssemblyName));
            }

            base.OnConfiguring(dbContextOptionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            // one to many relationship
            modelBuilder.Entity<ApplicationUser>()
                .ToTable("AspNetUsers", T => T.ExcludeFromMigrations())
                .HasMany<Group>()
                .WithOne(x => x.ApplicationUser);

            modelBuilder.Entity<Group>()
                .HasMany(g => g.ExcelFile)
                .WithOne(c => c.Group);

            modelBuilder.Entity<Group>()
                .HasMany(g => g.ExportFileHistory)
                .WithOne(c => c.Group);

            modelBuilder.Entity<Group>()
                .HasMany(g => g.ExcelData)
                .WithOne(E => E.Group);

            modelBuilder.Entity<ExcelData>()
                .HasMany(g => g.ExcelFieldData)
                .WithOne(E => E.ExcelData);

            base.OnModelCreating(modelBuilder);
        }

        public DbSet<Group> Group { get; set; }
        public DbSet<ExcelFieldData> ExcelFieldData { get; set; }
        public DbSet<ExcelData> ExcelData { get; set; }
        public DbSet<ExcelFile> ExcelFile { get; set; }
        public DbSet<ExportFileHistory> ExportFileHistory { get; set; }
    }
}
