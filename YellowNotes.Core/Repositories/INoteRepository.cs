using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using YellowNotes.Core.Dtos;
using YellowNotes.Core.Models;

namespace YellowNotes.Core.Repositories
{
    public interface INoteRepository
    {
        Task<Note> CreateNote(Note note, string email, CancellationToken cancellationToken);

        Task<Note> GetNote(int noteId, CancellationToken cancellationToken);

        Task<Tuple<int, IEnumerable<Note>>> GetNotes(int takeCount, int skipCount, string email,
            CancellationToken cancellationToken);

        Task<bool> UpdateNote(NoteDto note, CancellationToken cancellationToken);

        Task<bool> DeleteNote(int noteId, CancellationToken cancellationToken);
    }
}
