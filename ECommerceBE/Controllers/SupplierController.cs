using ECommerceBE.Controllers.Utilities;
using ECommerceBE.Data;
using ECommerceBE.Models;
using ECommerceBE.Repository.IRepository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ECommerceBE.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
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
        [ProducesResponseType(200,Type = typeof(List<Supplier>))]
        public async Task<IActionResult> GetSuppliersAsync()
        {
            return Ok(await _repo.GetAllItemsAsync());
        }

        [HttpPost]
        [ProducesResponseType(201, Type = typeof(Supplier))]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> PostSupplierAsync([FromBody] Supplier supplier)
        {
            if (supplier == null)
            {
                return BadRequest(ModelState);
            }

            if (await _repo.ItemExistsAsync(supplier.Name))
            {
                ModelState.AddModelError("", "This supplier already exists");
                return StatusCode(404, ModelState);
            }

            supplier.Password = UserUtilities.hashPassword(supplier.Password);

            if (!await _repo.CreateItemAsync(supplier))
            {
                ModelState.AddModelError("", "Something went wrong during the process");
                return StatusCode(500, ModelState);
            }

            return StatusCode(201, supplier);
        }

        [HttpGet("{supplierId:int}", Name = "GetSupplierAsync")]
        [ProducesResponseType(200, Type = typeof(Supplier))]
        [ProducesResponseType(404)]
        [ProducesDefaultResponseType]
        public async Task<IActionResult> GetSupplierAsync(int supplierId)
        {
            var supplier = await _repo.GetItemAsync(supplierId);

            if (supplier == null)
            {
                return NotFound();
            }

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

        [HttpPut("{supplierId:int}", Name = "UpdateSupplierAsync")]
        [ProducesResponseType(204)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdateSupplierAsync(int supplierId, [FromBody] Supplier supplier)
        {
            if (supplier == null || supplierId <= 0)
            {
                return BadRequest(ModelState);
            }

            if (!await _repo.ItemExistsAsync(supplierId))
            {
                return NotFound();
            }

            supplier.Password = UserUtilities.hashPassword(supplier.Password);
            await _repo.UpdateItemAsync(supplier);

            return NoContent();
        }

        [HttpDelete("{supplierId:int}", Name = "DeleteSupplierAsync")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeleteSupplierAsync(int supplierId)
        {
            if (!await _repo.ItemExistsAsync(supplierId))
            {
                return NotFound();
            }

            var supplier = await _repo.GetItemAsync(supplierId);

            if (!await _repo.DeleteItemAsync(supplier))
            {
                ModelState.AddModelError("", $"Something went wrong during the process");
                return StatusCode(500, ModelState);
            }

            return NoContent();
        }
    }
}
