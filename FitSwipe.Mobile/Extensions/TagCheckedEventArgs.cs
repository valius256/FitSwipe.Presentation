using FitSwipe.Shared.Dtos.Others;
using FitSwipe.Shared.Dtos.Tags;

namespace FitSwipe.Mobile.Extensions
{
    public class TagCheckedEventArgs
    {
        public List<GetTagDto> Tags { get; set; }

        public TagCheckedEventArgs(List<GetTagDto> tags)
        {
            Tags = tags;
        }
    }
}
