using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using YellowNotes.Core.Dtos;
using YellowNotes.Core.Models;

namespace YellowNotes.Core.Repositories
{
    public interface INoteRepository
    {
        Task<bool> CreateNote(Note note, CancellationToken cancellationToken);

        Task<IEnumerable<Note>> GetNotes(int count, CancellationToken cancellationToken);

        Task<bool> UpdateNote(NoteDto note, CancellationToken cancellationToken);

        Task<bool> DeleteNote(int noteId, CancellationToken cancellationToken);
    }
}