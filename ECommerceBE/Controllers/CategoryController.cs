using ECommerceBE.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using ECommerceBE.Models;
using System.Linq;

namespace ECommerceBE.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CategoryController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public CategoryController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<List<Category>>> GetCategories()
        {
            return Ok(await _context.Categories.ToListAsync());
        }

        [HttpPost]
        public async Task<ActionResult> PostCategories(Category category)
        {
            _context.Categories.Add(category);
            await _context.SaveChangesAsync();
            return Ok(category);
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<Category>> GetCategory(int id)
        {
            var category = await _context.Categories.FirstOrDefaultAsync(x => x.Id == id);
            if (category == null)
                return NotFound("Category not found.");
            return Ok(category);
        }

        [HttpGet("{name}")]
        public async Task<ActionResult<List<Category>>> SearchByName(string name)
        {
            var categories = await _context.Categories.Where(data => data.Name.Contains(name)).ToListAsync();
            if (categories.Count == 0)
            {
                return NotFound("No results");
            }
            return categories;
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> UpdateCategory(int categoryId, [FromBody] Category category)
        {
            if (category == null)
            {
                return BadRequest(ModelState);
            }

            _context.Categories.Update(category);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult> DeleteCategory(int id)
        {
            var categoryExists = await _context.Categories.FirstOrDefaultAsync(x => x.Id == id);

            if (categoryExists == null)
            {
                return NotFound();
            }

            _context.Remove(categoryExists);
            await _context.SaveChangesAsync();
            return Ok();
        }
    }
}
