using ECommerceBE.Data;
using ECommerceBE.Models;
using ECommerceBE.Repository.IRepository;
using System.Collections.Generic;
using System.Linq;

namespace ECommerceBE.Repository
{
    public class SupplierRepository : IRepository<Supplier>
    {
        private readonly ApplicationDbContext _context;

        public SupplierRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public bool CreateItem(Supplier item)
        {
            _context.Suppliers.Add(item);
            return Save();
        }

        public bool DeleteItem(Supplier item)
        {
            _context.Suppliers.Remove(item);
            return Save();
        }

        public ICollection<Supplier> GetAllItems()
        {
            return _context.Suppliers.OrderBy(a => a.Name).ToList();
        }

        public Supplier GetItem(int id)
        {
            return _context.Suppliers.FirstOrDefault(a => a.Id == id);
        }

        public bool ItemExists(string name)
        {
            var result = _context.Suppliers.Any(a => a.Name.ToLower().Trim() == name.ToLower().Trim());
            return result;
        }

        public bool ItemExists(int id)
        {
            var result = _context.Suppliers.Any(a => a.Id == id);
            return result;
        }

        public bool Save()
        {
            return _context.SaveChanges() >= 0 ? true : false;
        }

        public bool UpdateItem(Supplier item)
        {
            _context.Suppliers.Update(item);
            return Save();
        }
    }
}
