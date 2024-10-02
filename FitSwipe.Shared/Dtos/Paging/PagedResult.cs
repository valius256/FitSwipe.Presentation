
namespace FitSwipe.Shared.Dtos.Paging
{
    public class PagedResult<T>
    {
        public IList<T> Items { get; set; }
        public string[] Errors { get; set; }
        public int Limit { get; set; }
        public int Page { get; set; }
        public int Total { get; set; }
    }
}
