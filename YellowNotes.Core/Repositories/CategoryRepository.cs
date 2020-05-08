using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using YellowNotes.Core.Models;

namespace YellowNotes.Core.Repositories
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly DatabaseContext context;

        public CategoryRepository(DatabaseContext context) => this.context = context;

        public Task<Category> CreateCategory(Category category, string email,
            CancellationToken cancellationToken)
        {
            throw new System.NotImplementedException();
        }

        public Task<object> DeleteCategory(int categoryId, string email,
            CancellationToken cancellationToken)
        {
            throw new System.NotImplementedException();
        }

        public Task<IEnumerable<Category>> GetCategories(string email,
            CancellationToken cancellationToken)
        {
            throw new System.NotImplementedException();
        }
    }
}
