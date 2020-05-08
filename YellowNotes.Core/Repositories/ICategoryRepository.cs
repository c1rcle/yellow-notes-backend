using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using YellowNotes.Core.Dtos;

namespace YellowNotes.Core.Repositories
{
    public interface ICategoryRepository
    {
        Task<CategoryDto> CreateCategory(CategoryDto category, string email,
            CancellationToken cancellationToken);

        Task<IEnumerable<CategoryDto>> GetCategories(string email,
            CancellationToken cancellationToken);

        Task<object> DeleteCategory(int categoryId, string email,
            CancellationToken cancellationToken);
    }
}
