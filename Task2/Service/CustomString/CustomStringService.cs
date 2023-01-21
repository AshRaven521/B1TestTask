using System.Collections.Generic;
using System.Threading.Tasks;
using Task2.Model;
using Task2.Repository;

namespace Task2.Service
{
    public class CustomStringService : ICustomStringService
    {
        private ICustomStringRepository repository;

        public CustomStringService(ICustomStringRepository repository)
        {
            this.repository = repository;
        }

        public async Task<IEnumerable<CustomString>> GetCustomStringsAsync()
        {
            return await repository.GetCustomStringsAsync();
        }

        public async Task<CustomString> GetCustomStringByIdAsync(int id)
        {
            return await repository.GetCustomStringByIdAsync(id);
        }

        public async Task<CustomString> InsertCustomStringAsync(CustomString custom)
        {
            return await repository.InsertCustomStringAsync(custom);
        }

        public async Task DeleteCustomStringAsync(int id)
        {
            await repository.DeleteCustomStringAsync(id);
        }

        public async Task SaveAsync()
        {
            await repository.SaveAsync();
        }

        public async Task InsertCustomStringsAsync(List<CustomString> customs)
        {
            await repository.InsertCustomStringsAsync(customs);
        }
    }
}
