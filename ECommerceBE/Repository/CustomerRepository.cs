using ECommerceBE.Data;
using ECommerceBE.Models;
using ECommerceBE.Repository.IRepository;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ECommerceBE.Repository
{
    public class CustomerRepository : IRepository<Customer>
    {
        private readonly ApplicationDbContext _context;

        public CustomerRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<bool> CreateItemAsync(Customer item)
        {
            _context.Customers.Add(item);
            return await SaveAsync();
        }

        public async Task<bool> DeleteItemAsync(Customer item)
        {
            _context.Customers.Remove(item);
            return await SaveAsync();
        }

        public async Task<ICollection<Customer>> GetAllItemsAsync()
        {
            return await _context.Customers.OrderBy(a => a.Name).ToListAsync();
        }

        public async Task<ICollection<Customer>> SearchItemsAsync(string name)
        {
            List<Customer> customers = new List<Customer>();
            return customers;
        }

        public async Task<Customer> GetItemAsync(int id)
        {
            return await _context.Customers.FirstOrDefaultAsync(a => a.Id == id);
        }

        public async Task<bool> ItemExistsAsync(string name)
        {
            return await _context.Customers.AnyAsync(a => a.Name == name);
        }

        public async Task<bool> ItemExistsAsync(int id)
        {
            return await _context.Customers.AnyAsync(a => a.Id == id);
        }

        public async Task<bool> SaveAsync()
        {
            return await _context.SaveChangesAsync() >= 0 ? true : false;
        }

        public async Task<bool> UpdateItemAsync(Customer item)
        {
            _context.Customers.Update(item);
            return await SaveAsync();
        }
    }
}
