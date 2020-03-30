using AutoMapper;
using YellowNotes.Core.Models;

namespace YellowNotes.Core.Dtos
{
    public class Mapping : Profile
    {
        public Mapping()
        {
            CreateMap<Note, NoteDto>();
        }
    }
}
