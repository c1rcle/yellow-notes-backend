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

        public async Task<Note> CreateNote(Note note, string email,
            CancellationToken cancellationToken)
        {
            var user = await context.Users
                .SingleOrDefaultAsync(x => x.Email == email, cancellationToken);

            note.UserId = user.UserId;
            note.ModificationDate = DateTime.Now;
            note.IsRemoved = false;

            context.Notes.Add(note);

            bool success;
            try
            {
                success = await context.SaveChangesAsync(cancellationToken) > 0;
            }
            catch (DbUpdateException)
            {
                return null;
            }
            return success ? note : null;
        }

        public async Task<object> GetNote(int noteId, string email,
            CancellationToken cancellationToken)
        {
            var note = await context.Notes.Include(x => x.User)
                .SingleOrDefaultAsync(x => x.NoteId == noteId && x.IsRemoved == false,
                    cancellationToken);

            if (note == null)
            {
                return null;
            }
            else if (note.User.Email != email)
            {
                return "Requested resource is not available";
            }
            return note;
        }

        public async Task<Tuple<int, IEnumerable<Note>>> GetNotes(int takeCount, int skipCount,
            string email, CancellationToken cancellationToken)
        {
            var count = await context.Notes
                .CountAsync(x => x.User.Email == email && x.IsRemoved == false);
            var notes = await context.Notes.Where(x => x.User.Email == email && x.IsRemoved == false)
                .OrderByDescending(x => x.ModificationDate)
                .Skip(skipCount)
                .Take(takeCount)
                .AsNoTracking()
                .ToListAsync(cancellationToken);

            return Tuple.Create(count, notes as IEnumerable<Note>);
        }

        public async Task<object> UpdateNote(NoteDto note, string email,
            CancellationToken cancellationToken)
        {
            var record = await context.Notes.Include(x => x.User)
                .SingleOrDefaultAsync(x => x.NoteId == note.NoteId && x.IsRemoved == false,
                    cancellationToken);

            if (record == null)
            {
                return null;
            }
            else if (record.User.Email != email)
            {
                return "Requested resource cannot be updated";
            }

            record.ModificationDate = DateTime.Now;
            record.Title = note.Title ?? record.Title;
            record.Content = note.Content ?? record.Content;
            record.ImageUrl = note.ImageUrl ?? record.ImageUrl;
            record.Color = note.Color ?? record.Color;
            record.Category = note.Tags ?? record.Category;
            record.IsBlocked = note.IsBlocked;

            return await context.SaveChangesAsync(cancellationToken) > 0;
        }

        public async Task<object> DeleteNote(int noteId, string email,
            CancellationToken cancellationToken)
        {
            var record = await context.Notes.Include(x => x.User)
                .SingleOrDefaultAsync(x => x.NoteId == noteId && x.IsRemoved == false,
                    cancellationToken);

            if (record == null)
            {
                return null;
            }
            else if (record.User.Email != email)
            {
                return "Requested resource cannot be deleted!";
            }

            record.IsRemoved = true;
            return await context.SaveChangesAsync(cancellationToken) > 0;
        }
    }
}
