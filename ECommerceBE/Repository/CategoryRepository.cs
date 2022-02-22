using ECommerceBE.Data;
using ECommerceBE.Models;
using ECommerceBE.Repository.IRepository;
using System.Collections.Generic;
using System.Linq;

namespace ECommerceBE.Repository
{
    public class CategoryRepository : IRepository<Category>
    {
        private readonly ApplicationDbContext _context;

        public CategoryRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public bool CreateItem(Category item)
        {
            _context.Categories.Add(item);
            return Save();
        }

        public bool DeleteItem(Category item)
        {
            _context.Categories.Remove(item);
            return Save();
        }

        public ICollection<Category> GetAllItems()
        {
            return _context.Categories.OrderBy(a => a.Name).ToList();
        }

        public Category GetItem(int id)
        {
            return _context.Categories.FirstOrDefault(a => a.Id == id);
        }

        public bool ItemExists(string name)
        {
            var result = _context.Categories.Any(a => a.Name.ToLower().Trim() == name.ToLower().Trim());
            return result;
        }

        public bool ItemExists(int id)
        {
            var result = _context.Categories.Any(a => a.Id == id);
            return result;
        }

        public bool Save()
        {
            return _context.SaveChanges() >= 0 ? true : false;
        }

        public bool UpdateItem(Category item)
        {
            _context.Categories.Update(item);
            return Save();
        }
    }
}
