using System.Collections.Generic;
using System.Threading.Tasks;
using Task2.Model;

namespace Task2.Repository
{
    internal interface IBalanceRepository
    {
        Task<IEnumerable<Balance>> GetBalancesAsync();
        Task<Balance> GetBalanceByIdAsync(int id);
        Task<Balance> InsertBalanceAsync(Balance balance);
        Task InsertBalancesAsync(List<Balance> balances);
        Task DeleteBalanceAsync(int id);
        Task SaveAsync();
    }
}
