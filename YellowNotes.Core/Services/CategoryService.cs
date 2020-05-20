using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using YellowNotes.Core.Dtos;
using YellowNotes.Core.Models;
using YellowNotes.Core.Repositories;
using YellowNotes.Core.Utility;

namespace YellowNotes.Core.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository repository;

        private readonly IMapper mapper;

        private readonly int maxCategoryCount = 10;

        public CategoryService(ICategoryRepository repository, IMapper mapper)
        {
            this.repository = repository;
            this.mapper = mapper;
        }

        public async Task<CategoryDto> CreateCategory(CategoryDto category, string email,
            CancellationToken cancellationToken)
        {
            var count = await repository.GetCategoryCount(email, cancellationToken);
            if (count < maxCategoryCount)
            {
                var mappedCategory = mapper.Map<Category>(category);
                var result = await repository.CreateCategory(mappedCategory, email, cancellationToken);
                return mapper.Map<CategoryDto>(result);
            }

            return null;
        }

        public async Task<ResultHandler> GetCategory(int categoryId, string email,
            CancellationToken cancellationToken)
        {
            var category = await repository.GetCategory(categoryId, cancellationToken);

            if (category == null)
            {
                return new ResultHandler(HttpStatusCode.NotFound);
            }
            else if (category.User.Email != email)
            {
                return new ResultHandler(HttpStatusCode.Unauthorized,
                    "Requested resource is not available!");
            }
            return new ResultHandler(HttpStatusCode.OK, mapper.Map<CategoryDto>(category));
        }

        public async Task<IEnumerable<CategoryDto>> GetCategories(string email,
            CancellationToken cancellationToken)
        {
            var categories = await repository.GetCategories(email, cancellationToken);
            return categories.Select(x => mapper.Map<CategoryDto>(x));
        }

        public async Task<ResultHandler> DeleteCategory(int categoryId, string email,
            CancellationToken cancellationToken)
        {
            var category = await repository.GetCategory(categoryId, cancellationToken);

            if (category == null)
            {
                return new ResultHandler(HttpStatusCode.NotFound);
            }
            else if (category.User.Email != email)
            {
                return new ResultHandler(HttpStatusCode.Unauthorized,
                    "Requested resource is not available!");
            }

            var success = await repository.DeleteCategory(categoryId, email, cancellationToken);

            return success
                ? new ResultHandler(HttpStatusCode.NoContent)
                : new ResultHandler(HttpStatusCode.NotFound);
        }
    }
}
