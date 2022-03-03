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
    public class CustomerController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IRepository<Customer> _repo;

        public CustomerController(ApplicationDbContext context, IRepository<Customer> repo)
        {
            _context = context;
            _repo = repo;
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(List<Customer>))]
        public async Task<IActionResult> GetCustomersAsync()
        {
            return Ok(await _repo.GetAllItemsAsync());
        }

        [HttpPost]
        [ProducesResponseType(201, Type = typeof(Customer))]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> PostCustomerAsync(Customer customer)
        {
            if (customer == null)
            {
                return BadRequest(ModelState);
            }

            if (await _repo.ItemExistsAsync(customer.Name))
            {
                ModelState.AddModelError("", "This customer already exists");
                return StatusCode(404, ModelState);
            }

            customer.Password = UserUtilities.hashPassword(customer.Password);

            if (!await _repo.CreateItemAsync(customer))
            {
                ModelState.AddModelError("", "Something went wrong during the process");
                return StatusCode(500, ModelState);
            }

            return Ok(customer);
        }

        [HttpGet("{customerId:int}", Name = "GetCustomerAsync")]
        [ProducesResponseType(200, Type = typeof(Customer))]
        [ProducesResponseType(404)]
        [ProducesDefaultResponseType]
        public async Task<IActionResult> GetCustomerAsync(int customerId)
        {
            var customer = await _repo.GetItemAsync(customerId);

            if (customer == null)
            {
                return NotFound();
            }

            return Ok(customer);
        }

        [HttpGet("{name}")]
        public async Task<ActionResult<List<Customer>>> SearchByName(string name)
        {
            var customers = await _context.Customers.Where(data => data.Name.Contains(name)).ToListAsync();
            if (customers.Count == 0)
            {
                return NotFound("No results");
            }
            return customers;
        }

        [HttpPut("{customerId:int}", Name = "UpdateCustomerAsync")]
        [ProducesResponseType(204)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdateCustomerAsync(int customerId, [FromBody] Customer customer)
        {
            if (customer == null || customerId <= 0)
            {
                return BadRequest(ModelState);
            }

            if (!await _repo.ItemExistsAsync(customerId))
            {
                return NotFound();
            }

            customer.Password = UserUtilities.hashPassword(customer.Password);
            await _repo.UpdateItemAsync(customer);

            return NoContent();
        }

        [HttpDelete("{customerId:int}", Name = "DeleteCustomerAsync")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeleteCustomerAsync(int customerId)
        {
            if (!await _repo.ItemExistsAsync(customerId))
            {
                return NotFound();
            }

            var customer = await _repo.GetItemAsync(customerId);

            if (!await _repo.DeleteItemAsync(customer))
            {
                ModelState.AddModelError("", $"Something went wrong during the process");
                return StatusCode(500, ModelState);
            }

            return NoContent();
        }
    }
}
