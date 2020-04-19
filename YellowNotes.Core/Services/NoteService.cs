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

        public async Task<NoteDto> CreateNote(NoteDto note, string email,
            CancellationToken cancellationToken)
        {
            var mappedNote = mapper.Map<Note>(note);

            var result = await repository.CreateNote(mappedNote, email, cancellationToken);
            return mapper.Map<NoteDto>(result);
        }

        public async Task<object> GetNote(int noteId, string email,
            CancellationToken cancellationToken)
        {
            var note = await repository.GetNote(noteId, email, cancellationToken);
            return mapper.Map<NoteDto>(note);
        }

        public async Task<Tuple<int, IEnumerable<NoteDto>>> GetNotes(int takeCount, int skipCount,
            string email, CancellationToken cancellationToken)
        {
            var notes = await repository.GetNotes(takeCount, skipCount, email, cancellationToken);
            return Tuple.Create(notes.Item1, notes.Item2.Select(x => mapper.Map<NoteDto>(x)));
        }

        public async Task<object> UpdateNote(NoteDto note, string email, 
            CancellationToken cancellationToken)
        {
            return await repository.UpdateNote(note, email, cancellationToken);
        }

        public async Task<object> DeleteNote(int noteId, string email,
            CancellationToken cancellationToken)
        {
            return await repository.DeleteNote(noteId, email, cancellationToken);
        }
    }
}
