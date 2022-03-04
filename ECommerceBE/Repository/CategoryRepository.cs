using ECommerceBE.Data;
using ECommerceBE.Models;
using ECommerceBE.Repository.IRepository;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ECommerceBE.Repository
{
    public class CategoryRepository : IRepository<Category>
    {
        private readonly ApplicationDbContext _context;

        public CategoryRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<bool> CreateItemAsync(Category item)
        {
            _context.Categories.Add(item);
            return await SaveAsync();
        }

        public async Task<bool> DeleteItemAsync(Category item)
        {
            _context.Categories.Remove(item);
            return await SaveAsync();
        }

        public async Task<ICollection<Category>> GetAllItemsAsync()
        {
            return await _context.Categories.OrderBy(a => a.Name).ToListAsync();
        }

        public async Task<ICollection<Category>> SearchItemsAsync(string name)
        {
            List<Category> categories = new List<Category>();
            return categories;
        }

        public async Task<Category> GetItemAsync(int id)
        {
            return await _context.Categories.FirstOrDefaultAsync(a => a.Id == id);
        }

        public async Task<bool> ItemExistsAsync(string name)
        {
            return await _context.Categories.AnyAsync(a => a.Name.ToLower().Trim() == name.ToLower().Trim());
        }

        public async Task<bool> ItemExistsAsync(int id)
        {
            return await _context.Categories.AnyAsync(a => a.Id == id);
        }

        public async Task<bool> SaveAsync()
        {
            return await _context.SaveChangesAsync() >= 0 ? true : false;
        }

        public async Task<bool> UpdateItemAsync(Category item)
        {
            _context.Categories.Update(item);
            return await SaveAsync();
        }
    }
}
