using ECommerceBE.Controllers.Utilities;
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
    public class CustomerController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public CustomerController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<List<Customer>>> GetCustomers()
        {
            return Ok(await _context.Customers.ToListAsync());
        }

        [HttpPost]
        public async Task<ActionResult> PostCustomers(Customer customer)
        {
            customer.Password = UserUtilities.hashPassword(customer.Password);
            _context.Customers.Add(customer);
            await _context.SaveChangesAsync();
            return Ok(customer);
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<Customer>> GetCustomer(int id)
        {
            var customer = await _context.Customers.FirstOrDefaultAsync(x => x.Id == id);
            if (customer == null)
                return NotFound("Customers not found.");
            return Ok(customer);
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> UpdateCustomer(int customerId, [FromBody] Customer customer)
        {
            if (customer == null)
            {
                return BadRequest(ModelState);
            }

            customer.Password = UserUtilities.hashPassword(customer.Password);
            _context.Customers.Update(customer);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult> DeleteCustomers(int id)
        {
            var customerExists = await _context.Customers.FirstOrDefaultAsync(x => x.Id == id);

            if (customerExists == null)
            {
                return NotFound();
            }

            _context.Remove(customerExists);
            await _context.SaveChangesAsync();
            return Ok();
        }
    }
}
