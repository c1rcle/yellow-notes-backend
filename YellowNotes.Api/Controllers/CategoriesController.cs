using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using YellowNotes.Api.Extensions;
using YellowNotes.Core.Dtos;
using YellowNotes.Core.Services;

namespace YellowNotes.Api.Controllers
{
    [ApiController]
    [Route("/[controller]")]
    public class CategoriesController : ControllerBase
    {
        private readonly ICategoryService categoryService;

        public CategoriesController(ICategoryService categoryService) =>
            this.categoryService = categoryService;

        [HttpGet("{categoryId}")]
        [ProducesResponseType(typeof(CategoryDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetCategory(int categoryId,
            CancellationToken cancellationToken = default)
        {
            var userEmail = HttpContext.GetEmailFromClaims();
            var result = await categoryService
                .GetCategory(categoryId, userEmail, cancellationToken);

            return result.GetActionResult(this);
        }

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<CategoryDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetCategories(
            CancellationToken cancellationToken = default)
        {
            var userEmail = HttpContext.GetEmailFromClaims();
            var categories = await categoryService.GetCategories(userEmail, cancellationToken);
            return Ok(categories);
        }

        [HttpPost]
        [ProducesResponseType(typeof(CategoryDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
        public async Task<IActionResult> CreateCategory([FromBody] CategoryDto category,
            CancellationToken cancellationToken = default)
        {
            if (category.CategoryId != 0)
            {
                return BadRequest();
            }

            var userEmail = HttpContext.GetEmailFromClaims();
            var result = await categoryService
                .CreateCategory(category, userEmail, cancellationToken);

            if (result == null)
            {
                return UnprocessableEntity("Failed to create category");
            }

            return CreatedAtAction(nameof(GetCategory),
                new { categoryId = result.CategoryId }, result);
        }

        [HttpDelete("{categoryId}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteCategory(int categoryId,
            CancellationToken cancellationToken = default)
        {
            var userEmail = HttpContext.GetEmailFromClaims();
            var result = await categoryService
                .DeleteCategory(categoryId, userEmail, cancellationToken);

            return result.GetActionResult(this);
        }
    }
}
