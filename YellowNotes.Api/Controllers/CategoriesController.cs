using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using YellowNotes.Api.Extensions;
using YellowNotes.Core.Dtos;
using YellowNotes.Core.Repositories;

namespace YellowNotes.Api.Controllers
{
    [ApiController]
    [Route("/[controller]")]
    public class CategoriesController : ControllerBase
    {
        private readonly ICategoryRepository categoryRepository;

        public CategoriesController(ICategoryRepository categoryRepository) =>
            this.categoryRepository = categoryRepository;

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<CategoryDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetCategories(
            CancellationToken cancellationToken = default)
        {
            var userEmail = HttpContext.GetEmailFromClaims();
            var categories = await categoryRepository.GetCategories(userEmail, cancellationToken);
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
            var result = await categoryRepository
                .CreateCategory(category, userEmail, cancellationToken);

            if (result == null)
            {
                return UnprocessableEntity("Failed to create category");
            }
            return Ok(result);
        }

        [HttpDelete("{categoryId}")]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteCategory(int categoryId,
            CancellationToken cancellationToken = default)
        {
            var userEmail = HttpContext.GetEmailFromClaims();
            var result = await categoryRepository
                .DeleteCategory(categoryId, userEmail, cancellationToken);

            if (result is string)
            {
                return Unauthorized(result);
            }
            else
            {
                return (bool)result
                    ? NoContent() as IActionResult
                    : NotFound();
            }
        }
    }
}
