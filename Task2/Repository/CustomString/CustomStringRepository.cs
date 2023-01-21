using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Task2.Model;
using Task2.Data;
using Microsoft.EntityFrameworkCore;

namespace Task2.Repository
{
    public class CustomStringRepository : ICustomStringRepository
    {
        private ApplicationDbContext context;

        public CustomStringRepository(ApplicationDbContext context)
        {
            this.context = context;
        }

        public async Task DeleteCustomStringAsync(int id)
        {
            var csToDelete = await context.CustomStrings.FindAsync(id);
            context.CustomStrings.Remove(csToDelete);
            await context.SaveChangesAsync();
        }

        public async Task<CustomString> GetCustomStringByIdAsync(int id)
        {
            var cs = await context.CustomStrings.FindAsync(id);
            return cs;
        }

        public async Task<IEnumerable<CustomString>> GetCustomStringsAsync()
        {
            return await context.CustomStrings.ToListAsync();
        }

        public async Task<CustomString> InsertCustomStringAsync(CustomString custom)
        {
            var newCS = await context.CustomStrings.AddAsync(custom);
            await context.SaveChangesAsync();
            return newCS.Entity;
        }

        public async Task InsertCustomStringsAsync(List<CustomString> customs)
        {
            await context.CustomStrings.AddRangeAsync(customs);
            await context.SaveChangesAsync();
        }

        public async Task SaveAsync()
        {
            await context.SaveChangesAsync();
        }
    }
}
