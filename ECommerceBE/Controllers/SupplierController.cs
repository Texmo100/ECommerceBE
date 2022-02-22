using ECommerceBE.Controllers.Utilities;
using ECommerceBE.Data;
using ECommerceBE.Models;
using ECommerceBE.Repository.IRepository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ECommerceBE.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SupplierController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IRepository<Supplier> _repo;

        public SupplierController(ApplicationDbContext context, IRepository<Supplier> repo)
        {
            _context = context;
            _repo = repo;
        }

        [HttpGet]
        public ActionResult<List<Supplier>> GetSuppliers()
        {
            return Ok(_repo.GetAllItems());
        }

        [HttpPost]
        public async Task<ActionResult> PostSuppliers(Supplier supplier)
        {
            supplier.Password = UserUtilities.hashPassword(supplier.Password);
            _context.Add(supplier);
            await _context.SaveChangesAsync();
            return Ok(supplier);
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<Supplier>> GetSupplier(int id)
        {
            var supplier = await _context.Suppliers.FirstOrDefaultAsync(x => x.Id == id);
            if (supplier == null)
                return NotFound("Supplier not found.");
            return Ok(supplier);
        }

        [HttpGet("{name}")]
        public async Task<ActionResult<List<Supplier>>> SearchByName(string name)
        {
            var suppliers = await _context.Suppliers.Where(data => data.Name.Contains(name)).ToListAsync();
            if (suppliers.Count == 0)
            {
                return NotFound("No results");
            }
            return suppliers;
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> UpdateSupplier(int supplierId, [FromBody] Supplier supplier)
        {
            if (supplier == null)
            {
                return BadRequest(ModelState);
            }

            supplier.Password = UserUtilities.hashPassword(supplier.Password);
            _context.Suppliers.Update(supplier);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult> DeleteSupplier(int id)
        {
            var supplierExists = await _context.Suppliers.FirstOrDefaultAsync(x => x.Id == id);

            if (supplierExists == null)
            {
                return NotFound();
            }

            _context.Remove(supplierExists);
            await _context.SaveChangesAsync();
            return Ok();
        }
    }
}
