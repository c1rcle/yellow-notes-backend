using YellowNotes.Core.Utility;

namespace YellowNotes.Core.Dtos
{
    public class NoteQueryDto : PagingRequest
    {
        public int[] CategoryIds { get; set; } = new int[0];
    }
}
