using CommunityToolkit.Maui.Core.Extensions;
using FitSwipe.Mobile.Pages.ProfilePages;
using FitSwipe.Shared.Dtos.Chats;
using FitSwipe.Shared.Dtos.Paging;
using FitSwipe.Shared.Dtos.Trainings;
using FitSwipe.Shared.Dtos.Users;
using FitSwipe.Shared.Enums;
using FitSwipe.Shared.Utils;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Maui.Controls;
using System.Collections.ObjectModel;

namespace FitSwipe.Mobile.Pages.ChatPages;

public partial class ChatDetail : ContentPage
{
    public GetUserDto LoginedUser { get; set; }
    public GetSimpleUserChatRoomDto Guest { get; set; }
    public GetChatRoomDto ChatRoom { get; set; }

    private string _token;
    private HubConnection _hubConnection;
    private string _theme = "#52BB00";
    private int skip = 0;
    private int newMessages = 0;
    private int total = 1;
    private int pageSize = 30;
    private bool isVisiting = false;
    public string Theme
    {
        get => _theme;
        set
        {
            _theme = value;
            OnPropertyChanged(nameof(Theme));
        }
    }



    private ObservableCollection<GetMessageDto> _messages = new ObservableCollection<GetMessageDto>();

    public ObservableCollection<GetMessageDto> Messages
    {
        get => _messages;
        set
        {
            _messages = value;
            OnPropertyChanged(nameof(Messages));
        }
    }

    public ChatDetail(GetUserDto user, GetSimpleUserChatRoomDto guest, GetChatRoomDto chatRoom, string token)
    {
        InitializeComponent();
        LoginedUser = user;
        ChatRoom = chatRoom;
        Guest = guest;
        _token = token;
        InitializeSignalR();
        if (LoginedUser.Role == Role.PT)
        {
            Theme = "#2E3191";
        }
        
        FetchMessages();
        BindingContext = this;

        
    }
    private async void FetchMessages()
    {
        
        loadingDialog.IsVisible = true;
        try
        {
            var result = await Fetcher.GetAsync<PagedResult<GetMessageDto>>($"api/RealtimeChat/messages?guestId={Guest.UserFirebaseId}&skip={skip}&limit={pageSize}",_token);
            if (result == null)
            {
                throw new Exception("Không thể lấy dữ liệu đoạn chat");
            }
            Messages = new ObservableCollection<GetMessageDto>();
            total = result.Total;
            skip += result.Items.Count;
            foreach (var message in result.Items)
            {
                FormatMessages(new List<GetMessageDto> { message });
                Messages.Insert(0,message);
                //await Task.Delay(100);
            }
            var lastIndex = messagesList.ItemsSource.Cast<object>().Count() - 1;
            messagesList.ScrollTo(lastIndex, position: ScrollToPosition.End, animate: false);
        } catch (Exception ex)
        {
            await DisplayAlert("Lỗi","Có lỗi xảy ra.\n" + ex.Message,"OK");
            await Navigation.PopModalAsync();
        }
        loadingDialog.IsVisible = false;

    }
    private void FormatMessages(List<GetMessageDto> messages)
    {
        var yourColor = LoginedUser.Role == Role.Trainee ? "#52BB00" : "#2E3191";
        var thierColor = Guest.Role == Role.Trainee ? "#52BB00" : "#2E3191";
        foreach (var message in messages)
        {
            if (message.UserFirebaseId == LoginedUser.FireBaseId)
            {
                message.Margin = "End";
                message.BackgroundColor = yourColor;
                message.TextColor = "White";
                message.BorderColor = yourColor;
                message.AvatarColor = yourColor;
                message.RightAvatarSource = LoginedUser.AvatarUrl ?? "profile";
            } else
            {
                message.Margin = "Start";
                message.BackgroundColor = "White";
                message.TextColor = "Black";
                message.BorderColor = thierColor;
                message.AvatarColor = thierColor;
                message.LeftAvatarSource = Guest.AvatarUrl ?? "profile";
            }
        }
    }
    private async void InitializeSignalR()
    {
        // opt => opt.Transports = Microsoft.AspNetCore.Http.Connections.HttpTransportType.WebSockets
        // Initialize SignalR connection to the server
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
                if (chatRoomId == ChatRoom.ChatRoomId.ToString())
                {
                    var messageDto = new GetMessageDto { UserFirebaseId = userId, Content = message, SentAt = sendAt };
                    FormatMessages(new List<GetMessageDto> { messageDto });
                    Messages.Add(messageDto);
                    skip += 1;
                    newMessages += 1;
                }               
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

                    var chatRoomId = ChatRoom.ChatRoomId;
                    if (!string.IsNullOrWhiteSpace(chatRoomId.ToString()))
                    {
                        await _hubConnection.InvokeAsync("JoinRoom", chatRoomId, false, user.FireBaseId);
                    }
                }
            }
        }
        catch (Exception ex)
        {
            //await DisplayAlert("Lỗi", $"Lỗi kết nối: {ex.Message}", "OK");
        }
    }
    protected override void OnAppearing()
    {
        base.OnAppearing();
        isVisiting = false;
    }
    protected override async void OnDisappearing()
    {
        base.OnDisappearing();
        if (!isVisiting)
        {
            if (_hubConnection != null)
            {
                await _hubConnection.StopAsync();
            }
            var role = LoginedUser.Role == Role.Trainee ? "Trainee" : "PT";
            await Shell.Current.GoToAsync($"//ChatPage?role={role}&flag=false");
        }
    }

    public void Dispose()
    {
        _hubConnection?.DisposeAsync();
    }

    private async void tappedSend_Tapped(object sender, TappedEventArgs e)
    {
        var token = await SecureStorage.GetAsync("auth_token");
        if (token != null)
        {
            var user = await Shortcut.GetLoginedUser(token);
            var message = txtMessage.Text;
            if (!string.IsNullOrWhiteSpace(message))
            {
                try
                {
                    sendingMessage.IsVisible = true;
                    txtMessage.Text = string.Empty; // Clear the input box
                    // Send the message to the SignalR hub
                    //await _hubConnection.InvokeAsync("SendMessageToAll", LoginedUser.FireBaseId , message );
                    await _hubConnection.InvokeAsync("SendMessage", ChatRoom.ChatRoomId , LoginedUser.FireBaseId , message );
                    sendingMessage.IsVisible = false;

                    //await _hubConnection.SendAsync("SendMessage", "47813622-dd53-40da-9726-a22fc48a718a", message);
                }
                catch (Exception ex)
                {
                    await DisplayAlert("Lỗi",$"Gửi thất bại: {ex.Message}","OK");
                }
            }
        }
    }

    private async void btnBack_Clicked(object sender, EventArgs e)
    {
        await Navigation.PopModalAsync();
    }

    private async void btnMoreMessage_Clicked(object sender, EventArgs e)
    {
        if (!loadingDialog.IsVisible)
        {
            showMoreMessageSection.IsVisible = false;
            loadingMessage.IsVisible = true;
            try
            {
                var result = await Fetcher.GetAsync<PagedResult<GetMessageDto>>($"api/RealtimeChat/messages?guestId={Guest.UserFirebaseId}&skip={skip}&limit={pageSize}", _token);
                if (result == null)
                {
                    throw new Exception("Không thể lấy dữ liệu đoạn chat");
                }
                skip += result.Items.Count;
                foreach (var message in result.Items)
                {
                    FormatMessages(new List<GetMessageDto> { message });
                    Messages.Insert(0, message);
                    //await Task.Delay(100);
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Lỗi", "Có lỗi xảy ra.\n" + ex.Message, "OK");
            }
            if (skip - newMessages < total)
            {
                showMoreMessageSection.IsVisible = true;
            }
            loadingMessage.IsVisible = false;
        }

    }

    private async void tapAvatar_Tapped(object sender, TappedEventArgs e)
    {
        isVisiting = true;
        if (ChatRoom.Users[0].Role == Role.PT)
        {
            await Navigation.PushModalAsync(new PTProfilePage(ChatRoom.Users[0].UserFirebaseId));
        }
        else
        {
            await Navigation.PushModalAsync(new UserProfilePage(ChatRoom.Users[0].UserFirebaseId));
        }
    }
}