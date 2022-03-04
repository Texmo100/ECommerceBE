using ECommerceBE.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using ECommerceBE.Models;
using System.Linq;
using Microsoft.AspNetCore.Http;
using ECommerceBE.Repository.IRepository;

namespace ECommerceBE.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public class CategoryController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IRepository<Category> _repo;

        public CategoryController(ApplicationDbContext context, IRepository<Category> repo)
        {
            _context = context;
            _repo = repo;
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(List<Category>))]
        public async Task<IActionResult> GetCategoriesAsync()
        {
            return Ok(await _repo.GetAllItemsAsync());
        }

        [HttpPost]
        [ProducesResponseType(201, Type = typeof(Category))]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> PostCategoryAsync(Category category)
        {
            if (category == null)
            {
                return BadRequest(ModelState);
            }

            if (await _repo.ItemExistsAsync(category.Name))
            {
                ModelState.AddModelError("", "This category already exists");
                return StatusCode(404, ModelState);
            }

            if (!await _repo.CreateItemAsync(category))
            {
                ModelState.AddModelError("", "Something went wrong during the process");
                return StatusCode(500, ModelState);
            }

            return StatusCode(201, category);
        }

        [HttpGet("{categoryId:int}", Name = "GetCategoryAsync")]
        [ProducesResponseType(200, Type = typeof(Category))]
        [ProducesResponseType(404)]
        [ProducesDefaultResponseType]
        public async Task<IActionResult> GetCategoryAsync(int categoryId)
        {
            var category = await _repo.GetItemAsync(categoryId);

            if (category == null)
            {
                return NotFound();
            }

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

        [HttpPut("{categoryId:int}", Name = "UpdateCategoryAsync")]
        [ProducesResponseType(204)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdateCategoryAsync(int categoryId, [FromBody] Category category)
        {
            if (category == null || categoryId <= 0)
            {
                return BadRequest(ModelState);
            }

            if (!await _repo.ItemExistsAsync(categoryId))
            {
                return NotFound();
            }

            await _repo.UpdateItemAsync(category);

            return NoContent();
        }

        [HttpDelete("{categoryId:int}", Name = "DeleteCategoryAsync")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeleteCategoryAsync(int categoryId)
        {
            if (!await _repo.ItemExistsAsync(categoryId))
            {
                return NotFound();
            }

            var category = await _repo.GetItemAsync(categoryId);

            if (!await _repo.DeleteItemAsync(category))
            {
                ModelState.AddModelError("", $"Something went wrong during the process");
                return StatusCode(500, ModelState);
            }

            return NoContent();
        }
    }
}
