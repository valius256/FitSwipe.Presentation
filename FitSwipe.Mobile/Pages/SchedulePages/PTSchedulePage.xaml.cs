using CommunityToolkit.Maui.Core.Extensions;
using FitSwipe.Mobile.Controls;
using FitSwipe.Shared.Dtos.Paging;
using FitSwipe.Shared.Dtos.Slots;
using FitSwipe.Shared.Dtos.Users;
using FitSwipe.Shared.Enums;
using FitSwipe.Shared.Utils;
using Mapster;
using System.Collections.ObjectModel;

namespace FitSwipe.Mobile.Pages.SchedulePages;

public partial class PTSchedulePage : ContentPage
{
    public ObservableCollection<GetSlotDto> Slots { get; set; } = new ObservableCollection<GetSlotDto>();
    private GetUserDto? LoginedUser;
    public PTSchedulePage()
    {
        InitializeComponent();
        Setup();
        pageContent.IsVisible = false;
    }

    private async void Setup()
    {
        var token = await SecureStorage.GetAsync("auth_token");
        if (token != null)
        {
            LoginedUser = await Shortcut.GetLoginedUser(token);
        }
        await FetchSlots(true);
        pageContent.IsVisible = true;
    }

    private async Task FetchSlots(bool isInitial)
    {
        loadingDialog.IsVisible = true;
        loadingDialog.Message = "Đang lấy dữ liệu...";
        try
        {
            if (LoginedUser == null)
            {
                throw new Exception("Người dùng chưa đăng nhập!");
            }
            var start = isInitial ? Helper.GetFirstDayOfWeek() : timeTable.CurrentWeek.StartDate.ToDateTime(TimeOnly.MinValue);
            var end = isInitial ? Helper.GetLastDayOfWeek() : timeTable.CurrentWeek.EndDate.ToDateTime(TimeOnly.MaxValue);
            string url = $"api/Slot?Filter.CreateById={LoginedUser.FireBaseId}&Filter.StartTime={start.ToString("yyyy-MM-ddTHH:mm:ssZ")}&Filter.EndTime={end.ToString("yyyy-MM-ddTHH:mm:ssZ")}&limit=500";
            var result = await Fetcher.GetAsync<PagedResult<GetSlotDto>>(url);
            if (result != null)
            {
                Slots = result.Items.ToObservableCollection();
                ColorSlots();
                timeTable.SetSlots(Slots);
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("Lỗi","Có lỗi xảy ra. Err : " + ex.Message,"OK");
        }
        loadingDialog.IsVisible = false;
    }
    public void ColorSlots()
    {
        foreach (var slot in Slots)
        {
            if (slot.Status == SlotStatus.Unbooked)
            {
                slot.Color = "#2E3192";
                if (slot.StartTime < DateTime.Now)
                {
                    slot.Color = "#636362";
                }
            } 
            else if (slot.Status == SlotStatus.Disabled)
            {
                slot.Color = "#000000";
            }
            else if (slot.Status == SlotStatus.OnGoing)
            {
                slot.Color = "#e3a702";
            }
            else if (slot.Status == SlotStatus.NotStarted)
            {
                slot.Color = "#2bad1a";
            }
            else if (slot.Status == SlotStatus.Finished)
            {
                slot.Color = "#0fab60";
            }
        }
    }
    private void btnClose_Clicked(object sender, EventArgs e)
    {

    }

    private async void timeTable_WeekChanged(object sender, EventArgs e)
    {
        await FetchSlots(false);
        //var timeTable = sender as TimeTable;
        //if (timeTable != null) 
        //{
        //    await DisplayAlert("Selected Week",$"{timeTable.CurrentWeek.Display}","OKE");
        //}
    }

    private void btnAdd_Tapped(object sender, TappedEventArgs e)
    {
        addSlotModal.Show();
    }

    private void btnDuplicate_Tapped(object sender, TappedEventArgs e)
    {
        duplicateSlotModal.Show();
    }

    private async void addSlotModal_OnAdded(object sender, EventArgs e)
    {
        var answer = await DisplayAlert("Thêm khung giờ", "Bạn có chắc chắn về hành động này?", "Có", "Không");
        if (answer)
        {
            var addModal = sender as PTAddSlotModal;
            if (addModal != null)
            {
                var timeFrames = addModal.GetTimeFrame();
                if (timeFrames[0] >= timeFrames[1])
                {
                    await DisplayAlert("Lỗi", "Vui lòng chọn thời gian bắt đầu nhỏ hơn thời gian kết thúc", "OK");
                } 
                else if (timeFrames[0].Hour < 4 || timeFrames[1].Hour > 22)
                {
                    await DisplayAlert("Lỗi", "Vui lòng chọn khung thời gian từ 4h - 22h", "OK");
                } 
                else if (timeFrames[0] <= DateTime.Now)
                {
                    await DisplayAlert("Lỗi", "Vui lòng chọn khung thời gian chưa diễn ra", "OK");
                }
                else if (Helper.IsConflict(timeFrames[0], timeFrames[1], Slots))
                {
                    await DisplayAlert("Lỗi", "Đã bị trùng giờ. Vui lòng kiểm tra lại", "OK");
                }
                else
                {
                    loadingDialog.IsVisible = true;
                    try
                    {
                        loadingDialog.Message = "Đang thực hiện...";
                        var token = await SecureStorage.GetAsync("auth_token");
                        if (token != null)
                        {
                            var slots = await Fetcher.PostAsync<List<RequestCreateSlotDto>,List<GetSlotDto>>("api/Slot/create", new List<RequestCreateSlotDto>
                            {
                                new RequestCreateSlotDto
                                {
                                    StartTime = DateTime.SpecifyKind(timeFrames[0],DateTimeKind.Utc),
                                    EndTime = DateTime.SpecifyKind(timeFrames[1],DateTimeKind.Utc),
                                }                                
                            }, token);
                            if (slots != null)
                            {
                                Slots.Add(slots[0]);
                                ColorSlots();
                                timeTable.SetSlots(Slots);
                            }                          
                            addModal.Hide();
                            timeTable.GotoWeek(DateOnly.FromDateTime(timeFrames[0]));
                        }
                    }
                    catch (Exception ex)
                    {
                        await DisplayAlert("Lỗi","Có lỗi đã xảy ra. Err : " + ex.Message,"OK");
                    }
                    loadingDialog.IsVisible = false;
                }
            }
        }

    }

    private async void btnDelete_Tapped(object sender, TappedEventArgs e)
    {
        var answer = await DisplayAlert("Xóa hết khung giờ của tuần này", "Bạn có chắc chắn về hành động này?", "Có", "Không");
        if (answer)
        {
            //Test delete slot
            Slots.Clear();
            timeTable.SetSlots(Slots);
        }
    }

    private async void editSlotModal_OnAdded(object sender, EventArgs e)
    {
        var answer = await DisplayAlert("Chỉnh khung giờ", "Bạn có chắc chắn về hành động này?", "Có", "Không");
        if (answer)
        {
            var editModal = sender as PTAddSlotModal;
            if (editModal != null)
            {
                var slot = Slots.FirstOrDefault(s => editModal.RefSlot != null && s.Id == editModal.RefSlot.Id);
                var timeFrames = editModal.GetTimeFrame();
                if (timeFrames[0] >= timeFrames[1])
                {
                    await DisplayAlert("Lỗi", "Vui lòng chọn thời gian bắt đầu nhỏ hơn thời gian kết thúc", "OK");
                }
                else if (timeFrames[0].Hour < 4 || timeFrames[1].Hour > 22)
                {
                    await DisplayAlert("Lỗi", "Vui lòng chọn khung thời gian từ 4h - 22h", "OK");
                }
                else if (timeFrames[0] <= DateTime.Now)
                {
                    await DisplayAlert("Lỗi", "Vui lòng chọn khung thời gian chưa diễn ra", "OK");
                }
                else if (Helper.IsConflict(timeFrames[0], timeFrames[1], Slots, slot))
                {
                    await DisplayAlert("Lỗi", "Đã bị trùng giờ. Vui lòng kiểm tra lại", "OK");
                }
                else
                {
                    loadingDialog.IsVisible = true;
                    try
                    {
                        loadingDialog.Message = "Đang thực hiện...";
                        var token = await SecureStorage.GetAsync("auth_token");
                        if (token != null)
                        {
                            if (slot == null)
                            {
                                throw new Exception("Buổi này không còn tồn tại");
                            }
                            await Fetcher.PutAsync("api/Slot/time", new RequestUpdateSlotTimeDto
                            {
                                SlotId = slot.Id,
                                StartTime = DateTime.SpecifyKind(timeFrames[0], DateTimeKind.Utc),
                                EndTime = DateTime.SpecifyKind(timeFrames[1], DateTimeKind.Utc),
                            }, token);

                            slot.StartTime = timeFrames[0];
                            slot.EndTime = timeFrames[1];
                            timeTable.SetSlots(Slots);

                            editModal.Hide();
                            timeTable.GotoWeek(DateOnly.FromDateTime(timeFrames[0]));
                        }
                    }
                    catch (Exception ex)
                    {
                        await DisplayAlert("Lỗi", "Có lỗi đã xảy ra. Err : " + ex.Message, "OK");
                    }
                    loadingDialog.IsVisible = false;
                }
            }
        }
    }

    private void timeTable_SlotAction(object sender, SlotEventArgs e)
    {
        var border = e.Border;
        var slot = e.Slot;

        border.GestureRecognizers.Clear();
        var tapGesture = new TapGestureRecognizer();
        tapGesture.Tapped += (sender, e) =>
        {
            editSlotModal.RefSlot = slot;
            editSlotModal.Show();
        };
        border.GestureRecognizers.Add(tapGesture);
    }

    private async void editSlotModal_OnDeleted(object sender, EventArgs e)
    {
        var answer = await DisplayAlert("Xóa khung giờ", "Bạn có chắc chắn về hành động này?", "Có", "Không");
        if (answer)
        {
            var editModal = sender as PTAddSlotModal;
            if (editModal != null)
            {
                loadingDialog.IsVisible = true;
                try
                {
                    loadingDialog.Message = "Đang thực hiện...";
                    var token = await SecureStorage.GetAsync("auth_token");
                    if (token != null)
                    {
                        var slot = Slots.FirstOrDefault(s => editModal.RefSlot != null && s.Id == editModal.RefSlot.Id);
                        if (slot == null)
                        {
                            throw new Exception("Buổi này không còn tồn tại");
                        }
                        await Fetcher.DeleteAsync("api/Slot/" + slot.Id, token);
                        Slots.Remove(slot);
                        timeTable.SetSlots(Slots);
                        editModal.Hide();
                    }
                }
                catch (Exception ex)
                {
                    await DisplayAlert("Lỗi", "Có lỗi đã xảy ra. Err : " + ex.Message, "OK");
                }
                loadingDialog.IsVisible = false;
            }

        }
    }

    private async void duplicateSlotModal_OnConfirmed(object sender, PTDuplicatingSlotModal.WeekCheckedEventArgs e)
    {
        var weeks = e.Weeks;
        if (weeks != null && weeks.Count > 0 && LoginedUser != null)
        {
            var answer = await DisplayAlert("Nhân bản các buổi tuần này sang tuần khác", "Bạn có chắc chắn về hành động này?", "Có", "Không");
            if (answer)
            {
                loadingDialog.IsVisible = true;
                loadingDialog.Message = "Đang thực hiện... Vui lòng chờ lát nhé!";
                try
                {
                    var newSlots = new List<RequestCreateSlotDto>();
                    //Fetch slots in the range
                    var startDay = weeks.First().StartDate.ToDateTime(TimeOnly.MinValue);
                    var endDay = weeks.Last().EndDate.ToDateTime(TimeOnly.MaxValue);
                    string url = $"api/Slot?Filter.CreateById={LoginedUser.FireBaseId}&Filter.StartTime={startDay.ToString("yyyy-MM-ddTHH:mm:ssZ")}&Filter.EndTime={endDay.ToString("yyyy-MM-ddTHH:mm:ssZ")}&limit=500";
                    var result = await Fetcher.GetAsync<PagedResult<GetSlotDto>>(url);
                    var currentWeek = timeTable.CurrentWeek;

                    //Add new slots to the list sequentially
                    if (result != null)
                    {
                        var slots = result.Items.Adapt<ObservableCollection<GetSlotDto>>();
                        foreach (var week in weeks)
                        {
                            foreach (var slot in Slots)
                            {
                                int dayDistance = (int) (week.StartDate.ToDateTime(TimeOnly.MinValue) - currentWeek.StartDate.ToDateTime(TimeOnly.MinValue)).TotalDays;
                                var newSlot = new RequestCreateSlotDto
                                {
                                    StartTime = slot.StartTime.AddDays(dayDistance),
                                    EndTime = slot.EndTime.AddDays(dayDistance)
                                };
                                if (Helper.IsConflict(newSlot.StartTime, newSlot.EndTime, slots))
                                {
                                    throw new Exception("Đã bị trùng giờ. Vui lòng kiểm tra lại");
                                }
                                newSlots.Add(newSlot);                                                               
                            }
                        }
                    }
                    //Now add them to database
                    var token = await SecureStorage.GetAsync("auth_token");
                    if (token == null)
                    {
                        throw new Exception("Người dùng chưa đăng nhập hoặc phiên đã hết hạn");
                    }
                    var addedSlots = await Fetcher.PostAsync<List<RequestCreateSlotDto>, List<GetSlotDto>>("api/Slot/create", newSlots, token);
                    if (addedSlots != null)
                    {
                        foreach(var slot in addedSlots)
                        {
                            Slots.Add(slot);
                        }
                        ColorSlots();
                        timeTable.SetSlots(Slots);
                        duplicateSlotModal.Hide();
                        await DisplayAlert("Thành công", "Đã copy sang các tuần được chọn", "OK");
                    }
                } 
                catch (Exception ex)
                {
                    await DisplayAlert("Lỗi", "Có lỗi xảy ra. Err : " + ex.Message, "OK");
                }
                loadingDialog.IsVisible = false;
            }
        }
    }
}