using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using YellowNotes.Core.Dtos;

namespace YellowNotes.Core.Services
{
    public interface INoteService
    {
        Task<bool> CreateNote(NoteDto note, CancellationToken cancellationToken);

        Task<IEnumerable<NoteDto>> GetNotes(int count, CancellationToken cancellationToken);

        Task<bool> UpdateNote(int noteId, CancellationToken cancellationToken);

        Task<bool> DeleteNote(int noteId, CancellationToken cancellationToken);
    }
}
