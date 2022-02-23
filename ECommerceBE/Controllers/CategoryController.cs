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
        public IActionResult GetCategories()
        {
            return Ok(_repo.GetAllItems());
        }

        [HttpPost]
        [ProducesResponseType(201, Type = typeof(Category))]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult PostCategories(Category category)
        {
            if (category == null)
            {
                return BadRequest(ModelState);
            }

            if (_repo.ItemExists(category.Name))
            {
                ModelState.AddModelError("", "This category already exists");
                return StatusCode(404, ModelState);
            }

            if (!_repo.CreateItem(category))
            {
                ModelState.AddModelError("", "Something went wrong during the process");
                return StatusCode(500, ModelState);
            }

            return StatusCode(201, category);
        }

        [HttpGet("{categoryId:int}", Name = "GetCategory")]
        [ProducesResponseType(200, Type = typeof(Category))]
        [ProducesResponseType(404)]
        [ProducesDefaultResponseType]
        public IActionResult GetCategory(int categoryId)
        {
            var category = _repo.GetItem(categoryId);

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

        [HttpPut("{categoryId:int}", Name = "UpdateCategory")]
        [ProducesResponseType(204)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult UpdateCategory(int categoryId, [FromBody] Category category)
        {
            if (category == null || categoryId <= 0)
            {
                return BadRequest(ModelState);
            }

            if (!_repo.ItemExists(categoryId))
            {
                return NotFound();
            }

            _repo.UpdateItem(category);

            return NoContent();
        }

        [HttpDelete("{categoryId:int}", Name = "DeleteCategory")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult DeleteCategory(int categoryId)
        {
            if (!_repo.ItemExists(categoryId))
            {
                return NotFound();
            }

            var category = _repo.GetItem(categoryId);

            if (!_repo.DeleteItem(category))
            {
                ModelState.AddModelError("", $"Something went wrong during the process");
                return StatusCode(500, ModelState);
            }

            return NoContent();
        }
    }
}
