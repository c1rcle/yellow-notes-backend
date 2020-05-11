using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using YellowNotes.Core.Models;

namespace YellowNotes.Core.Repositories
{
    public interface ICategoryRepository
    {
        Task<Category> CreateCategory(Category category, string email,
            CancellationToken cancellationToken);

        Task<Category> GetCategory(int categoryId, CancellationToken cancellationToken);

        Task<IEnumerable<Category>> GetCategories(string email,
            CancellationToken cancellationToken);

        Task<bool> DeleteCategory(int categoryId, string email,
            CancellationToken cancellationToken);

        Task<int> GetCategoryCount(string email, CancellationToken cancellationToken);
    }
}
