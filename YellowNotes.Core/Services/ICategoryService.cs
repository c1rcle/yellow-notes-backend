using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using YellowNotes.Core.Dtos;
using YellowNotes.Core.Utility;

namespace YellowNotes.Core.Services
{
    public interface ICategoryService
    {
        Task<CategoryDto> CreateCategory(CategoryDto category, string email,
            CancellationToken cancellationToken);

        Task<ResultHandler> GetCategory(int categoryId, string email,
            CancellationToken cancellationToken);

        Task<IEnumerable<CategoryDto>> GetCategories(string email,
            CancellationToken cancellationToken);

        Task<ResultHandler> DeleteCategory(int categoryId, string email,
            CancellationToken cancellationToken);
    }
}
