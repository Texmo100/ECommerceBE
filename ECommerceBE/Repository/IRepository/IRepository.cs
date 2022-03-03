using System.Collections.Generic;
using System.Threading.Tasks;

namespace ECommerceBE.Repository.IRepository
{
    public interface IRepository<T> where T : class
    {
        Task<ICollection<T>> GetAllItemsAsync();

        Task<T> GetItemAsync(int id);

        Task<bool> ItemExistsAsync(string name);

        Task<bool> ItemExistsAsync(int id);

        Task<bool> CreateItemAsync(T item);

        Task<bool> UpdateItemAsync(T item);

        Task<bool> DeleteItemAsync(T item);

        Task<bool> SaveAsync();
    }
}
