

namespace FitSwipe.Shared.Enums
{
    public enum SlotStatus
    {
        Unbooked,//Slot đang rảnh chưa ai book
        Pending, //Slot đã được book và đang chờ duyệt
        NotStarted,//Slot đã book và duyệt, đang chờ bắt đầu
        OnGoing,//Slot đã book và duyệt, đang diễn ra
        Finished,//Slot đã book và duyệt, đã hoàn thành
        Disabled//Slot bị vô hiệu hóa
    }
}
