using System.Linq;

namespace YellowNotes.Core.Utility
{
    public class GetNotesConfig
    {
        public int TakeCount { get; set; } = 20;

        public int SkipCount { get; set; } = 0;

        public int[] CategoryIds { get; set; } = new int[0];

        public bool IsCategorySelected(int? categoryId)
        {
            if (CategoryIds.Length > 0)
            {
                return categoryId.HasValue ? CategoryIds.Contains(categoryId.Value) : false;
            }
            return true;
        }
    }
}
