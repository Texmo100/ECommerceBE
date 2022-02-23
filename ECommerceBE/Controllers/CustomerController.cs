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
        public IActionResult GetCustomers()
        {
            return Ok(_repo.GetAllItems());
        }

        [HttpPost]
        [ProducesResponseType(201, Type = typeof(Customer))]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult PostCustomers(Customer customer)
        {
            if (customer == null)
            {
                return BadRequest(ModelState);
            }

            if (_repo.ItemExists(customer.Name))
            {
                ModelState.AddModelError("", "This customer already exists");
                return StatusCode(404, ModelState);
            }

            customer.Password = UserUtilities.hashPassword(customer.Password);

            if (!_repo.CreateItem(customer))
            {
                ModelState.AddModelError("", "Something went wrong during the process");
                return StatusCode(500, ModelState);
            }

            return StatusCode(201, customer);
        }

        [HttpGet("{customerId:int}", Name = "GetCustomer")]
        [ProducesResponseType(200, Type = typeof(Customer))]
        [ProducesResponseType(404)]
        [ProducesDefaultResponseType]
        public IActionResult GetCustomer(int customerId)
        {
            var customer = _repo.GetItem(customerId);

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

        [HttpPut("{customerId:int}", Name = "UpdateCustomer")]
        [ProducesResponseType(204)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult UpdateCustomer(int customerId, [FromBody] Customer customer)
        {
            if (customer == null || customerId <= 0)
            {
                return BadRequest(ModelState);
            }

            if (!_repo.ItemExists(customerId))
            {
                return NotFound();
            }

            customer.Password = UserUtilities.hashPassword(customer.Password);
            _repo.UpdateItem(customer);

            return NoContent();
        }

        [HttpDelete("{customerId:int}", Name = "DeleteCustomer")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult DeleteCustomers(int customerId)
        {
            if (!_repo.ItemExists(customerId))
            {
                return NotFound();
            }

            var customer = _repo.GetItem(customerId);

            if (!_repo.DeleteItem(customer))
            {
                ModelState.AddModelError("", $"Something went wrong during the process");
                return StatusCode(500, ModelState);
            }

            return NoContent();
        }
    }
}
