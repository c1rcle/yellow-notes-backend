using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using YellowNotes.Core.Dtos;
using YellowNotes.Core.Models;

namespace YellowNotes.Core.Repositories
{
    public class NoteRepository : INoteRepository
    {
        private readonly DatabaseContext context;

        public NoteRepository(DatabaseContext context) => this.context = context;

        public async Task<bool> CreateNote(Note note, CancellationToken cancellationToken)
        {
            note.ModificationDate = DateTime.Now;
            note.IsRemoved = false;

            context.Notes.Add(note);
            return await context.SaveChangesAsync(cancellationToken) > 0;
        }

        public async Task<IEnumerable<Note>> GetNotes(int takeCount, int skipCount, string email,
            CancellationToken cancellationToken)
        {
            return await context.Notes.Where(x => x.UserEmail == email && x.IsRemoved == false)
                .OrderByDescending(x => x.ModificationDate)
                .Skip(skipCount)
                .Take(takeCount)
                .AsNoTracking()
                .ToListAsync(cancellationToken);
        }

        public async Task<bool> UpdateNote(NoteDto note, CancellationToken cancellationToken)
        {
            var record = await context.Notes
                .SingleOrDefaultAsync(x => x.NoteId == note.NoteId, cancellationToken);
            
            if (record == null)
            {
                return false;
            }

            record.ModificationDate = DateTime.Now;
            record.Content = note.Content;
            
            context.Notes.Update(record);
            return await context.SaveChangesAsync(cancellationToken) > 0;
        }

        public async Task<bool> DeleteNote(int noteId, CancellationToken cancellationToken)
        {
            var record = await context.Notes
                .SingleOrDefaultAsync(x => x.NoteId == noteId, cancellationToken);
            
            if (record == null)
            {
                return false;
            }

            record.IsRemoved = true;

            context.Notes.Update(record);
            return await context.SaveChangesAsync(cancellationToken) > 0;
        }
    }
}
