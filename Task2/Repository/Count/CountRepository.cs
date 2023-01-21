using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using Task2.Data;
using Task2.Model;

namespace Task2.Repository
{
    public class CountRepository : ICountRepository
    {
    //    private ApplicationDbContext context;

    //    public CountRepository(ApplicationDbContext context)
    //    {
    //        this.context = context;
    //    }

    //    public async Task DeleteCountAsync(int id)
    //    {
    //        var countToDelete = await context.Counts.FindAsync(id);
    //        context.Counts.Remove(countToDelete);
    //        await context.SaveChangesAsync();
    //    }

    //    public async Task<Count> GetCountByIdAsync(int id)
    //    {
    //        var count = await context.Counts.FindAsync(id);
    //        return count;
    //    }

    //    public async Task<IEnumerable<Count>> GetCountsAsync()
    //    {
    //        return await context.Counts.ToListAsync();
    //    }

    //    public async Task<Count> InsertCountAsync(Count count)
    //    {
    //        var newCount = await context.Counts.AddAsync(count);
    //        await context.SaveChangesAsync();
    //        return newCount.Entity;
    //    }

    //    public async Task SaveAsync()
    //    {
    //        await context.SaveChangesAsync();
    //    }
    }
}
