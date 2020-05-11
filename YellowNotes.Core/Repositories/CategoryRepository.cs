using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using YellowNotes.Core.Models;

namespace YellowNotes.Core.Repositories
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly DatabaseContext context;

        public CategoryRepository(DatabaseContext context) => this.context = context;

        public async Task<Category> CreateCategory(Category category, string email,
            CancellationToken cancellationToken)
        {
            var user = await context.Users
                .SingleOrDefaultAsync(x => x.Email == email, cancellationToken);

            category.UserId = user.UserId;
            context.Categories.Add(category);

            var success = await context.SaveChangesAsync(cancellationToken) > 0;
            return success ? category : null;
        }

        public async Task<Category> GetCategory(int categoryId, CancellationToken cancellationToken)
        {
            return await context.Categories
                .Include(x => x.User)
                .SingleOrDefaultAsync(x => x.CategoryId == categoryId, cancellationToken);
        }

        public async Task<IEnumerable<Category>> GetCategories(string email,
            CancellationToken cancellationToken)
        {
            return await context.Categories.Where(x => x.User.Email == email)
                .OrderBy(x => x.Name)
                .AsNoTracking()
                .ToListAsync(cancellationToken);
        }

        public async Task<bool> DeleteCategory(int categoryId, string email,
            CancellationToken cancellationToken)
        {
            var record = await context.Categories.Include(x => x.User)
                .SingleOrDefaultAsync(x => x.CategoryId == categoryId, cancellationToken);

            context.Categories.Remove(record);
            return await context.SaveChangesAsync(cancellationToken) > 0;
        }

        public async Task<int> GetCategoryCount(string email, CancellationToken cancellationToken)
        {
            return await context.Categories
                .CountAsync(x => x.User.Email == email, cancellationToken);
        }
    }
}
