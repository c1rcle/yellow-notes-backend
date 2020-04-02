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

        public async Task<Note> CreateNote(Note note, CancellationToken cancellationToken)
        {
            note.ModificationDate = DateTime.Now;
            note.IsRemoved = false;

            context.Notes.Add(note);
            var success = await context.SaveChangesAsync(cancellationToken) > 0;
            return success ? note : null;
        }

        public async Task<Note> GetNote(int noteId, CancellationToken cancellationToken)
        {
            return await context.Notes.SingleOrDefaultAsync(x => x.NoteId == noteId,
                cancellationToken);
        }

        public async Task<Tuple<int, IEnumerable<Note>>> GetNotes(int takeCount, int skipCount,
            string email, CancellationToken cancellationToken)
        {
            var count = await context.Notes
                .CountAsync(x => x.UserEmail == email && x.IsRemoved == false);
            var notes = await context.Notes.Where(x => x.UserEmail == email && x.IsRemoved == false)
                .OrderByDescending(x => x.ModificationDate)
                .Skip(skipCount)
                .Take(takeCount)
                .AsNoTracking()
                .ToListAsync(cancellationToken);

            return Tuple.Create(count, notes as IEnumerable<Note>);
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
            record.Title = note.Title ?? record.Title;
            record.Content = note.Content ?? record.Content;

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
            return await context.SaveChangesAsync(cancellationToken) > 0;
        }
    }
}
