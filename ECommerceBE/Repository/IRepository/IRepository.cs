using System.Collections.Generic;

namespace ECommerceBE.Repository.IRepository
{
    public interface IRepository<T> where T : class
    {
        ICollection<T> GetAllItems();

        T GetItem(int id);

        bool ItemExists(string name);

        bool ItemExists(int id);

        bool CreateItem(T item);

        bool UpdateItem(T item);

        bool DeleteItem(T item);

        bool Save();
    }
}
