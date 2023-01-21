﻿using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using Task2.Data;
using Task2.Model;

namespace Task2.Repository
{
    public class BalanceRepository : IBalanceRepository
    {
        private ApplicationDbContext context;
        public BalanceRepository(ApplicationDbContext context)
        {
            this.context = context;
        }
        public async Task DeleteBalanceAsync(int id)
        {
            var balanceToDelete = await context.Balances.FindAsync(id);
            context.Balances.Remove(balanceToDelete);
            await context.SaveChangesAsync();
        }

        public async Task<Balance> GetBalanceByIdAsync(int id)
        {
            var balance = await context.Balances.FindAsync(id);
            return balance;
        }

        public async Task<IEnumerable<Balance>> GetBalancesAsync()
        {
            return await context.Balances.ToListAsync();
        }

        public async Task InsertBalancesAsync(List<Balance> balances)
        {
            await context.Balances.AddRangeAsync(balances);
            await context.SaveChangesAsync();
        }

        public async Task<Balance> InsertBalanceAsync(Balance balance)
        {
            var newBalance = await context.Balances.AddAsync(balance);
            await context.SaveChangesAsync();
            return newBalance.Entity;
        }

        public async Task SaveAsync()
        {
            await context.SaveChangesAsync();
        }
    }
}