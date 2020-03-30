using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using YellowNotes.Core.Dtos;
using YellowNotes.Core.Models;
using YellowNotes.Core.Repositories;

namespace YellowNotes.Core.Services
{
    public class NoteService : INoteService
    {
        private readonly INoteRepository repository;

        private readonly IMapper mapper;

        public NoteService(INoteRepository repository, IMapper mapper)
        {
            this.repository = repository;
            this.mapper = mapper;
        }

        public async Task<bool> CreateNote(NoteDto note, string email,
            CancellationToken cancellationToken)
        {
            return await repository.CreateNote(new Note
            {
                UserEmail = email,
                ModificationDate = DateTime.Now,
                Variant = note.Variant,
                IsRemoved = false,
                Content = note.Content
            }, cancellationToken);
        }

        public async Task<IEnumerable<NoteDto>> GetNotes(int count,
            CancellationToken cancellationToken)
        {
            var notes = await repository.GetNotes(count, cancellationToken);
            return notes.Select(x => mapper.Map<NoteDto>(x));
        }

        public async Task<bool> UpdateNote(NoteDto note, CancellationToken cancellationToken)
        {
            return await repository.UpdateNote(note, cancellationToken);
        }

        public async Task<bool> DeleteNote(int noteId, CancellationToken cancellationToken)
        {
            return await repository.DeleteNote(noteId, cancellationToken);
        }
    }
}
