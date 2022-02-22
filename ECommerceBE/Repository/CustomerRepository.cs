using ECommerceBE.Data;
using ECommerceBE.Models;
using ECommerceBE.Repository.IRepository;
using System.Collections.Generic;
using System.Linq;

namespace ECommerceBE.Repository
{
    public class CustomerRepository : IRepository<Customer>
    {
        private readonly ApplicationDbContext _context;

        public CustomerRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public bool CreateItem(Customer item)
        {
            _context.Customers.Add(item);
            return Save();
        }

        public bool DeleteItem(Customer item)
        {
            _context.Customers.Remove(item);
            return Save();
        }

        public ICollection<Customer> GetAllItems()
        {
            return _context.Customers.OrderBy(a => a.Name).ToList();
        }

        public Customer GetItem(int id)
        {
            return _context.Customers.FirstOrDefault(a => a.Id == id);
        }

        public bool ItemExists(string name)
        {
            var result = _context.Customers.Any(a => a.Name == name);
            return result;
        }

        public bool ItemExists(int id)
        {
            var result = _context.Customers.Any(a => a.Id == id);
            return result;
        }

        public bool Save()
        {
            return _context.SaveChanges() >= 0 ? true : false;
        }

        public bool UpdateItem(Customer item)
        {
            _context.Customers.Update(item);
            return Save();
        }
    }
}
