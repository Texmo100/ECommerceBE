using ECommerceBE.Data;
using ECommerceBE.Models;
using ECommerceBE.Repository.IRepository;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace ECommerceBE.Repository
{
    public class ProductRepository : IRepository<Product>
    {
        private readonly ApplicationDbContext _context;

        public ProductRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public bool CreateItem(Product item)
        {
            _context.Products.Add(item);
            return Save();
        }

        public bool DeleteItem(Product item)
        {
            _context.Products.Remove(item);
            return Save();
        }

        public ICollection<Product> GetAllItems()
        {
            return _context.Products.Include(c => c.Supplier).OrderBy(a => a.Name).ToList();
        }

        public Product GetItem(int id)
        {
            return _context.Products.Include(c => c.Supplier).FirstOrDefault(a => a.Id == id);
        }

        public bool ItemExists(string name)
        {
            var result = _context.Products.Any(a => a.Name.ToLower().Trim() == name.ToLower().Trim());
            return result;
        }

        public bool ItemExists(int id)
        {
            var result = _context.Products.Any(a => a.Id == id);
            return result;
        }

        public bool Save()
        {
            return _context.SaveChanges() >= 0 ? true : false;
        }

        public bool UpdateItem(Product item)
        {
            _context.Products.Update(item);
            return Save();
        }
    }
}
