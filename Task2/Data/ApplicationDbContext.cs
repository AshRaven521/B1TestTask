using Microsoft.EntityFrameworkCore;
using System.Linq;
using Task2.Model;
using Task2.Model.Files;

namespace Task2.Data
{
    public class ApplicationDbContext : DbContext
    {
        //public DbSet<Count> Counts { get; set; }
        public DbSet<Balance> Balances { get; set; }
        public DbSet<FileDetails> FileDetails { get; set; }
        public DbSet<CustomString> CustomStrings { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
            Database.EnsureCreated();
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            // Устанавливаем значение decimal(18, 2) для базы данных
            foreach (var property in builder.Model.GetEntityTypes()
                .SelectMany(t => t.GetProperties())
                .Where(p => p.ClrType == typeof(decimal)))
            {
                property.SetPrecision(18);
                property.SetScale(2);
            }

            //builder.Entity<Balance>().HasOne(b => b.Count).WithOne().HasForeignKey<Balance>(a => a.Count);
            //builder.Entity<Balance>().HasOne(b => b.File).WithOne().HasForeignKey<FileDetails>(a => a.BalanceId);
            //builder.Entity<FileDetails>().HasOne(b => b.Balance).WithOne().HasForeignKey<Balance>(a => a.FileId);
            //builder.Entity<FileDetails>().HasOne(c => c.CustomString).WithOne().HasForeignKey<CustomString>(b => b.FileId);
            builder.Entity<FileDetails>().HasMany<Balance>().WithOne().HasForeignKey(a => a.FileId);
            builder.Entity<FileDetails>().HasMany<CustomString>().WithOne().HasForeignKey(a => a.FileId);

        }
    }
}
