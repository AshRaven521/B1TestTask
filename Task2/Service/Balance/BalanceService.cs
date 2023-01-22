using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Task2.Model;
using Task2.Repository;

namespace Task2.Service
{
    internal class BalanceService : IBalanceService
    {
        private IBalanceRepository balanceRepository;

        public BalanceService(IBalanceRepository repository)
        {
            balanceRepository = repository;
        }

        public async Task DeleteBalanceAsync(int id)
        {
            await balanceRepository.DeleteBalanceAsync(id);
        }

        public async Task<Balance> GetBalanceByIdAsync(int id)
        {
            return await balanceRepository.GetBalanceByIdAsync(id);
        }

        public async Task<IEnumerable<Balance>> GetBalancesAsync()
        {
            return await balanceRepository.GetBalancesAsync();
        }

        public IQueryable<Balance> GetBalancesByFileId(int fileId)
        {
            return balanceRepository.GetBalancesByFileId(fileId);
        }

        public async Task<Balance> InsertBalanceAsync(Balance balance)
        {
            return await balanceRepository.InsertBalanceAsync(balance);
        }

        public async Task InsertBalancesAsync(List<Balance> balances)
        {
            await balanceRepository.InsertBalancesAsync(balances);
        }

        public async Task SaveAsync()
        {
            await balanceRepository.SaveAsync();
        }
    }
}
