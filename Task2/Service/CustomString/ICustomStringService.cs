using System.Collections.Generic;
using System.Threading.Tasks;
using Task2.Model;

namespace Task2.Service
{
    public interface ICustomStringService
    {
        Task<IEnumerable<CustomString>> GetCustomStringsAsync();
        Task<CustomString> GetCustomStringByIdAsync(int id);
        Task<CustomString> InsertCustomStringAsync(CustomString custom);
        Task InsertCustomStringsAsync(List<CustomString> customs);
        Task DeleteCustomStringAsync(int id);
        Task SaveAsync();

    }
}
