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
    public class ProductController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IRepository<Product> _repo;

        public ProductController(ApplicationDbContext context, IRepository<Product> repo)
        {
            _context = context;
            _repo = repo;
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(List<Product>))]
        public IActionResult GetProducts()
        {
            return Ok(_repo.GetAllItems());
        }

        [HttpPost]
        [ProducesResponseType(201, Type = typeof(Product))]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult PostProducts(Product product)
        {
            if (product == null)
            {
                return BadRequest(ModelState);
            }

            if (_repo.ItemExists(product.Name))
            {
                ModelState.AddModelError("", "This product already exists");
                return StatusCode(404, ModelState);
            }

            if (!_repo.CreateItem(product))
            {
                ModelState.AddModelError("", "Something went wrong during the process");
                return StatusCode(500, ModelState);
            }

            return StatusCode(201, product);
        }

        [HttpGet("{productId:int}", Name = "GetProduct")]
        [ProducesResponseType(200, Type = typeof(Product))]
        [ProducesResponseType(404)]
        [ProducesDefaultResponseType]
        public IActionResult GetProduct(int productId)
        {
            var product = _repo.GetItem(productId);

            if (product == null)
            {
                return NotFound();
            }

            return Ok(product);
        }

        [HttpGet("{name}", Name = "SearchByName")]
        [ProducesResponseType(200, Type = typeof(Product))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<List<Product>>> SearchByName(string name, [FromQuery] PaginationParams @params)
        {
            var products = await _context.Products.Where(data => data.Name.Contains(name))
                .Skip((@params.Page - 1) * @params.PageSize)
                .Take(@params.PageSize)
                .ToListAsync();
            if (products.Count == 0)
            {
                return NotFound("No results");
            }
            return products;
        }

        [HttpPut("{productId:int}", Name = "UpdateProduct")]
        [ProducesResponseType(204)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult UpdateProduct(int productId, [FromBody] Product product)
        {
            if (product == null || productId <= 0)
            {
                return BadRequest(ModelState);
            }

            if (!_repo.ItemExists(productId))
            {
                return NotFound();
            }

            _repo.UpdateItem(product);

            return NoContent();
        }

        [HttpDelete("{productId:int}", Name = "DeleteProduct")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult DeleteProduct(int productId)
        {
            if (!_repo.ItemExists(productId))
            {
                return NotFound();
            }

            var product = _repo.GetItem(productId);

            if (!_repo.DeleteItem(product))
            {
                ModelState.AddModelError("", $"Something went wrong during the process");
                return StatusCode(500, ModelState);
            }

            return NoContent();
        }
    }
}
