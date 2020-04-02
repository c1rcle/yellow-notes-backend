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

        Task<NoteDto> GetNote(int noteId, CancellationToken cancellationToken);

        Task<Tuple<int, IEnumerable<NoteDto>>> GetNotes(int takeCount, int skipCount, string email,
            CancellationToken cancellationToken);

        Task<bool> UpdateNote(NoteDto note, CancellationToken cancellationToken);

        Task<bool> DeleteNote(int noteId, CancellationToken cancellationToken);
    }
}
