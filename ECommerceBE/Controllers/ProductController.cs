using ECommerceBE.Data;
using ECommerceBE.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ECommerceBE.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public ProductController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<List<Product>>> GetProducts()
        {
            return Ok(await _context.Products.ToListAsync());
        }

        [HttpPost]
        public async Task<ActionResult> PostProducts(Product Product)
        {
            _context.Add(Product);
            await _context.SaveChangesAsync();
            return Ok(Product);
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<Product>> GetProduct(int id)
        {
            var product = await _context.Products.FirstOrDefaultAsync(x => x.Id == id);
            if (product == null)
                return NotFound("Product not found.");
            return Ok(product);
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult> DeleteProduct(int id)
        {
            var productExists = await _context.Products.FirstOrDefaultAsync(x => x.Id == id);

            if (productExists == null)
            {
                return NotFound();
            }

            _context.Remove(productExists);
            await _context.SaveChangesAsync();
            return Ok();
        }
    }
}
