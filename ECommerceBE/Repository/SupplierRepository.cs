using ECommerceBE.Data;
using ECommerceBE.Models;
using ECommerceBE.Repository.IRepository;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ECommerceBE.Repository
{
    public class SupplierRepository : IRepository<Supplier>
    {
        private readonly ApplicationDbContext _context;

        public SupplierRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<bool> CreateItemAsync(Supplier item)
        {
            _context.Suppliers.Add(item);
            return await SaveAsync();
        }

        public async Task<bool> DeleteItemAsync(Supplier item)
        {
            _context.Suppliers.Remove(item);
            return await SaveAsync();
        }

        public async Task<ICollection<Supplier>> GetAllItemsAsync()
        {
            return await _context.Suppliers.OrderBy(a => a.Name).ToListAsync();
        }

        public async Task<ICollection<Supplier>> SearchItemsAsync(string name)
        {
            List<Supplier> suppliers = new List<Supplier>();
            return suppliers;
        }

        public async Task<Supplier> GetItemAsync(int id)
        {
            return await _context.Suppliers.FirstOrDefaultAsync(a => a.Id == id);
        }

        public async Task<bool> ItemExistsAsync(string name)
        {
            return await _context.Suppliers.AnyAsync(a => a.Name.ToLower().Trim() == name.ToLower().Trim());
        }

        public async Task<bool> ItemExistsAsync(int id)
        {
            return await _context.Suppliers.AnyAsync(a => a.Id == id);
        }

        public async Task<bool> SaveAsync()
        {
            return await _context.SaveChangesAsync() >= 0 ? true : false;
        }

        public async Task<bool> UpdateItemAsync(Supplier item)
        {
            _context.Suppliers.Update(item);
            return await SaveAsync();
        }
    }
}
