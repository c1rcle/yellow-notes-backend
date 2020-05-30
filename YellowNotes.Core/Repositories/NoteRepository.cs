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

        public async Task<Note> GetNote(int noteId, string email,
            CancellationToken cancellationToken)
        {
            return await context.Notes
                .Include(x => x.User)
                .SingleOrDefaultAsync(x => x.NoteId == noteId && x.IsRemoved == false,
                    cancellationToken);
        }

        public async Task<IEnumerable<Note>> GetAllNotes(string email,
            CancellationToken cancellationToken)
        {
            return await context.Notes.Where(x => x.User.Email == email
                && x.IsRemoved == false)
                .OrderByDescending(x => x.ModificationDate)
                .AsNoTracking()
                .ToListAsync(cancellationToken);
        }

        public async Task<bool> UpdateNote(NoteDto note, string email,
            CancellationToken cancellationToken)
        {
            var record = await context.Notes.Include(x => x.User)
                .SingleOrDefaultAsync(x => x.NoteId == note.NoteId && x.IsRemoved == false,
                    cancellationToken);

            record.ModificationDate = DateTime.Now;
            record.Title = note.Title ?? record.Title;
            record.Content = note.Content ?? record.Content;
            record.ImageUrl = note.ImageUrl ?? record.ImageUrl;
            record.Color = note.Color ?? record.Color;
            record.IsBlocked = note.IsBlocked;

            if (note.CategoryId != null)
            {
                var category = await context.Categories.SingleOrDefaultAsync(
                    x => x.CategoryId == note.CategoryId && x.UserId == record.UserId,
                    cancellationToken);

                if (category == null)
                {
                    return false;
                }
            }

            record.CategoryId = note.CategoryId;
            return await context.SaveChangesAsync(cancellationToken) > 0;
        }

        public async Task<bool> DeleteNote(int noteId, string email,
            CancellationToken cancellationToken)
        {
            var record = await context.Notes
                .Include(x => x.User)
                .SingleOrDefaultAsync(x => x.NoteId == noteId && x.IsRemoved == false,
                    cancellationToken);

            record.IsRemoved = true;
            return await context.SaveChangesAsync(cancellationToken) > 0;
        }
    }
}
