using ECommerceBE.Data;
using ECommerceBE.Models;
using ECommerceBE.Repository.IRepository;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ECommerceBE.Repository
{
    public class ProductRepository : IRepository<Product>
    {
        private readonly ApplicationDbContext _context;

        public ProductRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<bool> CreateItemAsync(Product item)
        {
            _context.Products.Add(item);
            return await SaveAsync();
        }

        public async Task<bool> DeleteItemAsync(Product item)
        {
            _context.Products.Remove(item);
            return await SaveAsync();
        }

        public async Task<ICollection<Product>> GetAllItemsAsync()
        {
            return await _context.Products.Include(c => c.Supplier).OrderBy(a => a.Name).ToListAsync();
        }

        public async Task<ICollection<Product>> SearchItemsAsync(string name)
        {
            return await _context
                .Products
                .Where(data => data.Name.Contains(name))
                .ToListAsync();
        }

        public async Task<Product> GetItemAsync(int id)
        {
            return await _context.Products.Include(c => c.Supplier).FirstOrDefaultAsync(a => a.Id == id);
        }

        public async Task<bool> ItemExistsAsync(string name)
        {
            return await _context.Products.AnyAsync(a => a.Name.ToLower().Trim() == name.ToLower().Trim());
        }

        public async Task<bool> ItemExistsAsync(int id)
        {
            return await _context.Products.AnyAsync(a => a.Id == id);
        }

        public async Task<bool> SaveAsync()
        {
            return await _context.SaveChangesAsync() >= 0 ? true : false;
        }

        public async Task<bool> UpdateItemAsync(Product item)
        {
            _context.Products.Update(item);
            return await SaveAsync();
        }
    }
}
