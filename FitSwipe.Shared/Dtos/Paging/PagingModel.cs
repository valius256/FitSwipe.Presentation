
namespace FitSwipe.Shared.Dtos.Paging
{
    public class PagingModel<T> : PagingSizeModel
    {
        public T Filter { get; set; }
        //public List<SortItems> Sorts { get; set; }
    }

    public class PagingSizeModel
    {
        public required int Page { get; set; }

        public required int Limit { get; set; }
    }

    public class SortItems
    {
        public string Column { get; set; }
        public bool IsDesc { get; set; }
    }
}
