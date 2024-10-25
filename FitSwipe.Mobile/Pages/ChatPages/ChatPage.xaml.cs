using CommunityToolkit.Maui.Core.Extensions;
using FitSwipe.Shared.Dtos;
using FitSwipe.Shared.Dtos.Chats;
using FitSwipe.Shared.Dtos.Users;
using FitSwipe.Shared.Utils;
using Microsoft.AspNetCore.SignalR.Client;
using System.Collections.ObjectModel;

namespace FitSwipe.Mobile.Pages.ChatPages;

[QueryProperty(nameof(Role), "role")]
[QueryProperty(nameof(Flag), "flag")]
[QueryProperty(nameof(OpenId), "openId")]
public partial class ChatPage : ContentPage
{
    private HubConnection _hubConnection;
    private bool _isTrainee { get; set; }
    public bool IsTrainee 
    { 
        get => _isTrainee;
        set
        {
            _isTrainee = value;
            OnPropertyChanged(nameof(IsTrainee));
        }
    }
    private bool _isRefreshing { get; set; }
    public bool IsRefreshing
    {
        get => _isRefreshing;
        set
        {
            _isRefreshing = value;
            OnPropertyChanged(nameof(IsRefreshing));
        }
    }
    public string Role { get; set; }
    public bool Flag { get; set; }
    public string? OpenId { get; set; }
    private string _token;
    private ObservableCollection<GetChatRoomDto> _chatRooms { get; set; } = new ObservableCollection<GetChatRoomDto>();
    public ObservableCollection<GetChatRoomDto> ChatRooms
    {
        get => _chatRooms;
        set
        {
            _chatRooms = value;
            OnPropertyChanged();
        }
    }
    private List<GetChatRoomDto> OriginalChatRooms { get; set; } = new List<GetChatRoomDto>();
    public string Keyword { get; set; } = string.Empty;

    private GetUserDto _loginedUser = new();

    public ChatPage()
    {
        InitializeComponent();
        //ChatListView.ItemsSource = Messages; // Bind the ListView to the Messages collection
        InitializeSignalR();
        Setup();
        BindingContext = this;
    }
    private async void Setup()
    {
        try
        {
            var token = await SecureStorage.GetAsync("auth_token");
            if (token == null)
            {
                throw new Exception("Lỗi xác thực");
            }
            var user = await Shortcut.GetLoginedUser(token);

            if (user == null)
            {
                throw new Exception("Lỗi xác thực");

            }
            _token = token;
            _loginedUser = user;
            await CheckFlags();
            
        } catch(Exception ex)
        {
            await DisplayAlert("Lỗi", "Vui lòng đăng nhập lại!.\n" + ex.Message, "OK");
            await Shell.Current.GoToAsync("//SignIn");
        }
       
        if (!Flag)
        {
            await FetchChats();
        }
    }
    private async Task CheckFlags()
    {
        if (Role == "PT")
        {
            IsTrainee = false;
        }
        else
        {
            IsTrainee = true;
        }
        if (Flag)
        {
            Flag = false;
            await FetchChats();
        }
        if (OpenId != null)
        {
            await OpenChatRoomWith(OpenId);
            OpenId = null;
        }
    }
    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await CheckFlags();
    }
    private async Task FetchChats()
    {
        loadingDialog.IsVisible = true;
        try
        {
            var token = await SecureStorage.GetAsync("auth_token");
            if (token == null)
            {
                throw new Exception("Token is null");
            }
            var result = await Fetcher.GetAsync<List<GetChatRoomDto>>("api/RealtimeChat/getAllChatRooms", token);
            if (result != null)
            {
                ChatRooms = result.ToObservableCollection();
                OriginalChatRooms = ChatRooms.ToList();
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", ex.Message, "OK");
        }
        loadingDialog.IsVisible = false;
    }

    private async void InitializeSignalR()
    {
        _hubConnection = new HubConnectionBuilder()
            .WithUrl(Constant.BaseUrl + "chathub", opt =>
            {
                opt.HttpMessageHandlerFactory = _ => new HttpClientHandler
                {
                    ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator
                };

                opt.Transports = Microsoft.AspNetCore.Http.Connections.HttpTransportType.WebSockets |
                        Microsoft.AspNetCore.Http.Connections.HttpTransportType.LongPolling;

            })
            .WithAutomaticReconnect()
            .Build();

        // Set up the event handler to receive messages
        _hubConnection.On<string, string, DateTime, string>("ReceiveMessage", (userId, message, sendAt, chatRoomId) =>
        {
            MainThread.BeginInvokeOnMainThread(() =>
            {
                var chatRoom = ChatRooms.FirstOrDefault(cr => cr.ChatRoomId.ToString() == chatRoomId);
                if (chatRoom != null)
                {
                    ChatRooms.Remove(chatRoom);
                    OriginalChatRooms.Remove(chatRoom);
                    if (userId != _loginedUser.Id.ToString())
                    {
                        chatRoom.Users[1].UnseenMessaged += 1;
                    }
                    chatRoom.UpdatedDate = sendAt;
                    ChatRooms.Insert(0, chatRoom);
                    OriginalChatRooms.Insert(0,chatRoom);
                } 
                //else
                //{
                //    var newChatRoom = new GetChatRoomDto
                //    {
                //        ChatRoomId = Guid.Parse(chatRoomId),
                //        CreatedDate = sendAt,
                //        RoomName = "",
                //        UpdatedDate = sendAt,
                //        UserFirebaseId = userId,
                //    };
                //    FetchNewChatRoomDetail(newChatRoom);
                //}
            });
        });

        try
        {
            var token = await SecureStorage.GetAsync("auth_token");
            if (token != null)
            {
                var user = await Shortcut.GetLoginedUser(token);
                // Start the SignalR connection
                if (user != null)
                {
                    await _hubConnection.StartAsync();
                    var tasks = new List<Task>();
                    foreach (var item in ChatRooms)
                    {
                        if (!string.IsNullOrWhiteSpace(item.ChatRoomId.ToString()))
                        {
                            tasks.Add(_hubConnection.InvokeAsync("JoinRoom", item.ChatRoomId.ToString(), false, user.FireBaseId));
                        }
                    }
                    await Task.WhenAll(tasks);

                }
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("Lỗi", $"Lỗi kết nối: {ex.Message}", "OK");
        }
    }

    
    //private async void FetchNewChatRoomDetail(GetChatRoomDto getChatRoomDto)
    //{
    //    try
    //    {
    //        var result = await Fetcher.GetAsync<GetChatRoomDto>($"api/RealtimeChat/chat-room-detail/{getChatRoomDto.ChatRoomId}", _token);
    //        if (result != null) 
    //        {
    //            getChatRoomDto.Users = result.Users;
    //        }
    //        ChatRooms.Insert(0, getChatRoomDto);
    //        OriginalChatRooms.Insert(0, getChatRoomDto);
    //    } catch
    //    {
    //        //Can't not add
    //    }
    //}
   

    private async void chatItem_Tapped(object sender, TappedEventArgs e)
    {
        var border = sender as Border;
        if (border != null)
        {
            var tapGuesture = border.GestureRecognizers[0] as TapGestureRecognizer;
            if (tapGuesture != null)
            {
                var chatRoom = tapGuesture.CommandParameter as GetChatRoomDto;
                if (chatRoom != null && chatRoom.Users.Count > 0)
                {
                    loadingDialog.IsVisible = true;
                    if (chatRoom.Users[1].UnseenMessaged > 0)
                    {
                        try
                        {
                            await Fetcher.PutAsync($"api/RealtimeChat/seen?chatRoomId={chatRoom.ChatRoomId}", new BlankDto(), _token);
                            chatRoom.Users[1].UnseenMessaged = 0;
                        }
                        catch
                        {
                            await DisplayAlert("Lỗi", "Có lỗi khi lưu trạng thái xem", "OK");
                        }
                    }                  
                    await Navigation.PushModalAsync(new ChatDetail(_loginedUser, chatRoom.Users[0] , chatRoom, _token));
                    loadingDialog.IsVisible = false;
                }
            }
        }
    }

    private async Task OpenChatRoomWith(string guestId)
    {
        var chatRoom = ChatRooms.FirstOrDefault(cr => cr.Users.Count > 0 && cr.Users[0].UserFirebaseId == guestId);
        if (chatRoom != null)
        {
            await Navigation.PushModalAsync(new ChatDetail(_loginedUser, chatRoom.Users[0], chatRoom, _token));
        }
    }

    private void tappedSearch_Tapped(object sender, TappedEventArgs e)
    {
        //OriginalChatRooms = ChatRooms.ToObservableCollection();
        ChatRooms = OriginalChatRooms.Where(cr => cr.Users[0].UserName.Contains(Keyword)).ToObservableCollection();
    }

    private async void RefreshView_Refreshing(object sender, EventArgs e)
    {
        IsRefreshing = true;
        var exitTasks = new List<Task>();
        foreach (var item in ChatRooms)
        {
            if (!string.IsNullOrWhiteSpace(item.ChatRoomId.ToString()))
            {
                exitTasks.Add(_hubConnection.InvokeAsync("LeaveRoom", item.ChatRoomId.ToString()));
            }
        }

        await Task.WhenAll(exitTasks);
        await FetchChats();
        var enterTasks = new List<Task>();
        foreach (var item in ChatRooms)
        {
            if (!string.IsNullOrWhiteSpace(item.ChatRoomId.ToString()))
            {
                enterTasks.Add(_hubConnection.InvokeAsync("JoinRoom", item.ChatRoomId.ToString(), false, _loginedUser.FireBaseId));
            }
        }
        await Task.WhenAll(enterTasks);
        IsRefreshing = false;

    }
}