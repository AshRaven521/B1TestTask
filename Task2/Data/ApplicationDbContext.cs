using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Task2.Model;

namespace Task2.Data
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<Count> Counts { get; set; }
        public DbSet<Balance> Balances { get; set; }

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

            //builder.Entity<Balance>().HasOne(b => b.)
        }
    }
}
