using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using YellowNotes.Core.Dtos;
using YellowNotes.Core.Models;

namespace YellowNotes.Core.Repositories
{
    public class NoteRepository : INoteRepository
    {
        public async Task<bool> CreateNote(Note note, CancellationToken cancellationToken)
        {
            throw new System.NotImplementedException();
        }

        public async Task<IEnumerable<Note>> GetNotes(int count, CancellationToken cancellationToken)
        {
            throw new System.NotImplementedException();
        }

        public async Task<bool> UpdateNote(NoteDto note, CancellationToken cancellationToken)
        {
            throw new System.NotImplementedException();
        }

        public async Task<bool> DeleteNote(int noteId, CancellationToken cancellationToken)
        {
            throw new System.NotImplementedException();
        }
    }
}
