using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using YellowNotes.Core.Dtos;
using YellowNotes.Core.Models;

namespace YellowNotes.Core.Repositories
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly DatabaseContext context;

        private readonly IMapper mapper;

        private readonly int maxCategoryCount = 10;

        public CategoryRepository(DatabaseContext context, IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

        public async Task<CategoryDto> CreateCategory(CategoryDto category, string email,
            CancellationToken cancellationToken)
        {
            var mappedCategory = mapper.Map<Category>(category);
            var user = await context.Users
                .SingleOrDefaultAsync(x => x.Email == email, cancellationToken);

            var count = await context.Categories
                .CountAsync(x => x.User.Email == email, cancellationToken);

            if (count < maxCategoryCount)
            {
                mappedCategory.UserId = user.UserId;
                context.Categories.Add(mappedCategory);
            }

            var success = await context.SaveChangesAsync(cancellationToken) > 0;
            return success ? mapper.Map<CategoryDto>(mappedCategory) : null;
        }

        public async Task<IEnumerable<CategoryDto>> GetCategories(string email,
            CancellationToken cancellationToken)
        {
            var categories = await context.Categories.Where(x => x.User.Email == email)
                .OrderBy(x => x.Name)
                .AsNoTracking()
                .ToListAsync(cancellationToken);
            return categories.Select(x => mapper.Map<CategoryDto>(x));
        }

        public async Task<object> DeleteCategory(int categoryId, string email,
            CancellationToken cancellationToken)
        {
            var record = await context.Categories.Include(x => x.User)
                .SingleOrDefaultAsync(x => x.CategoryId == categoryId, cancellationToken);

            if (record == null)
            {
                return false;
            }
            else if (record.User.Email != email)
            {
                return "Requested resource cannot be deleted!";
            }

            context.Categories.Remove(record);
            return await context.SaveChangesAsync(cancellationToken) > 0;
        }
    }
}
