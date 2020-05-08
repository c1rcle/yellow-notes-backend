using System.Threading;
using System.Threading.Tasks;
using YellowNotes.Core.Dtos;
using YellowNotes.Core.Utility;

namespace YellowNotes.Core.Services
{
    public interface INoteService
    {
        Task<NoteDto> CreateNote(NoteDto note, string email, CancellationToken cancellationToken);

        Task<object> GetNote(int noteId, string email, CancellationToken cancellationToken);

        Task<NotesDto> GetNotes(GetNotesConfig config, string email,
            CancellationToken cancellationToken);

        Task<object> UpdateNote(NoteDto note, string email, CancellationToken cancellationToken);

        Task<object> DeleteNote(int noteId, string email, CancellationToken cancellationToken);
    }
}
