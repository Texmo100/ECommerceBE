using ECommerceBE.Data;
using ECommerceBE.Models;
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

        public SupplierController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<List<Supplier>>> GetSuppliers()
        {
            return Ok(await _context.Suppliers.ToListAsync());
        }

        [HttpPost]
        public async Task<ActionResult> PostSuppliers(Supplier supplier)
        {
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
