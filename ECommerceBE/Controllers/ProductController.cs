using ECommerceBE.Controllers.Utilities;
using ECommerceBE.Data;
using ECommerceBE.Models;
using ECommerceBE.Repository.IRepository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
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
        private readonly ILogger<Product> _logger;
        private readonly IRepository<Product> _repo;

        public ProductController(IRepository<Product> repo)
        {
            _repo = repo;
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(List<Product>))]
        public async Task<IActionResult> GetProductsAsync()
        {
            return Ok(await _repo.GetAllItemsAsync());
        }

        [HttpPost]
        [ProducesResponseType(201, Type = typeof(Product))]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> PostProductAsync(Product product)
        {
            if (product == null || product.SupplierId <= 0)
            {
                return BadRequest(ModelState);
            }

            if (await _repo.ItemExistsAsync(product.Name))
            {
                ModelState.AddModelError("", "This product already exists");
                return StatusCode(404, ModelState);
            }

            if (!await _repo.CreateItemAsync(product))
            {
                ModelState.AddModelError("", "Something went wrong during the process");
                return StatusCode(500, ModelState);
            }

            return StatusCode(201, product);
        }

        [HttpGet("{productId:int}", Name = "GetProductAsync")]
        [ProducesResponseType(200, Type = typeof(Product))]
        [ProducesResponseType(404)]
        [ProducesDefaultResponseType]
        public async Task<IActionResult> GetProductAsync(int productId)
        {
            var product = await _repo.GetItemAsync(productId);

            if (product == null)
            {
                return NotFound();
            }

            return Ok(product);
        }

        [HttpGet("{name}", Name = "SearchProductsByName")]
        [ProducesResponseType(200, Type = typeof(Product))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> SearchProductsByName(string name, [FromQuery] PaginationParams @params)
        {
            var matchedProducts = await _repo.SearchItemsAsync(name);

            if (matchedProducts.Count == 0)
            {
                return NotFound("No results");
            }

            int quotient = Math.DivRem(matchedProducts.Count, @params.PageSize, out int remainder);
            int numberOfPages = remainder == 0 ? quotient : quotient + 1;

            if (numberOfPages < @params.Page)
            {
                return BadRequest("This page does not have items");
            }

            var productsPaginated = matchedProducts
                .Skip((@params.Page - 1) * @params.PageSize)
                .Take(@params.PageSize);



            return Ok(new
            {
                matchedProducts = matchedProducts,
                countMatchedProducts = matchedProducts.Count,
                productsPaginated = productsPaginated,
                numberOfPages = numberOfPages
            });
        }


        [HttpPut("{productId:int}", Name = "UpdateProductAsync")]
        [ProducesResponseType(204)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdateProductAsync(int productId, [FromBody] Product product)
        {
            if (product == null || productId <= 0)
            {
                return BadRequest(ModelState);
            }

            if (!await _repo.ItemExistsAsync(productId))
            {
                return NotFound();
            }

            await _repo.UpdateItemAsync(product);

            return NoContent();
        }

        [HttpDelete("{productId:int}", Name = "DeleteProductAsync")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeleteProductAsync(int productId)
        {
            if (!await _repo.ItemExistsAsync(productId))
            {
                return NotFound();
            }

            var product = await _repo.GetItemAsync(productId);

            if (!await _repo.DeleteItemAsync(product))
            {
                ModelState.AddModelError("", $"Something went wrong during the process");
                return StatusCode(500, ModelState);
            }

            return NoContent();
        }
    }
}
