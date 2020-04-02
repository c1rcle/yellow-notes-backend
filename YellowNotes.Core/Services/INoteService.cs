using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using YellowNotes.Core.Dtos;

namespace YellowNotes.Core.Services
{
    public interface INoteService
    {
        Task<int?> CreateNote(NoteDto note, string email, CancellationToken cancellationToken);

        Task<NoteDto> GetNote(int noteId, CancellationToken cancellationToken);

        Task<IEnumerable<NoteDto>> GetNotes(int takeCount, int skipCount, string email,
            CancellationToken cancellationToken);

        Task<bool> UpdateNote(NoteDto note, CancellationToken cancellationToken);

        Task<bool> DeleteNote(int noteId, CancellationToken cancellationToken);
    }
}
