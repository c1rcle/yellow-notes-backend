using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using YellowNotes.Core.Dtos;

namespace YellowNotes.Core.Services
{
    public interface INoteService
    {
        Task<NoteDto> CreateNote(NoteDto note, string email, CancellationToken cancellationToken);

        Task<object> GetNote(int noteId, string email, CancellationToken cancellationToken);

        Task<Tuple<int, IEnumerable<NoteDto>>> GetNotes(int takeCount, int skipCount, string email,
            CancellationToken cancellationToken);

        Task<object> UpdateNote(NoteDto note, string email, CancellationToken cancellationToken);

        Task<object> DeleteNote(int noteId, string email, CancellationToken cancellationToken);
    }
}
