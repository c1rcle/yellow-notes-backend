using System.Threading;
using System.Threading.Tasks;
using YellowNotes.Core.Dtos;
using YellowNotes.Core.Models;
using YellowNotes.Core.Utility;

namespace YellowNotes.Core.Repositories
{
    public interface INoteRepository
    {
        Task<Note> CreateNote(Note note, string email, CancellationToken cancellationToken);

        Task<object> GetNote(int noteId, string email, CancellationToken cancellationToken);

        Task<NotesData> GetNotes(GetNotesConfig config, string email,
            CancellationToken cancellationToken);

        Task<object> UpdateNote(NoteDto note, string email, CancellationToken cancellationToken);

        Task<object> DeleteNote(int noteId, string email, CancellationToken cancellationToken);
    }
}
