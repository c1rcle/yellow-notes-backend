using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using YellowNotes.Core.Dtos;

namespace YellowNotes.Core.Services
{
    public class NoteService : INoteService
    {
        public Task<bool> CreateNote(NoteDto note, CancellationToken cancellationToken)
        {
            throw new System.NotImplementedException();
        }

        public Task<bool> DeleteNote(int noteId, CancellationToken cancellationToken)
        {
            throw new System.NotImplementedException();
        }

        public Task<IEnumerable<NoteDto>> GetNotes(int count, CancellationToken cancellationToken)
        {
            throw new System.NotImplementedException();
        }

        public Task<bool> UpdateNote(int noteId, CancellationToken cancellationToken)
        {
            throw new System.NotImplementedException();
        }
    }
}
