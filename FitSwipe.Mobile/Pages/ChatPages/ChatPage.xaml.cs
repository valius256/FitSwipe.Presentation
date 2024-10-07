using FitSwipe.Shared.Utils;
using Microsoft.AspNetCore.SignalR.Client;
using System.Collections.ObjectModel;

namespace FitSwipe.Mobile.Pages.ChatPages;

public partial class ChatPage : ContentPage
{
    private HubConnection _hubConnection;
    public ObservableCollection<MessageModel> Messages { get; set; }

    public ChatPage()
    {
        InitializeComponent();
        Messages = new ObservableCollection<MessageModel>();
        //ChatListView.ItemsSource = Messages; // Bind the ListView to the Messages collection
        InitializeSignalR();
        BindingContext = this;
    }

    private async void InitializeSignalR()
    {


        // opt => opt.Transports = Microsoft.AspNetCore.Http.Connections.HttpTransportType.WebSockets
        // Initialize SignalR connection to the server
        _hubConnection = new HubConnectionBuilder()
            .WithUrl("http://10.0.2.2:5250/chathub", opt =>
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
        _hubConnection.On<string, string>("ReceiveMessage", (userId, message) =>
        {
            MainThread.BeginInvokeOnMainThread(() =>
            {
                Messages.Add(new MessageModel { UserId = userId, Message = message });
                ChatListView.ScrollTo(Messages.Last(), position: ScrollToPosition.End, animated: true); // Scroll to the latest message
            });
        });

        try
        {


            var token = await SecureStorage.GetAsync("auth_token");
            var user = await Shortcut.GetLoginedUser(token);
            // Start the SignalR connection
            await _hubConnection.StartAsync();

            var chatRoomId = "";
            var isGroup = false;

            if (!string.IsNullOrWhiteSpace(chatRoomId) && isGroup == true)
            {
                await _hubConnection.InvokeAsync("JoinRoom", chatRoomId, false, user.FireBaseId);
            }

        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error starting connection: {ex.Message}");
        }
    }

    // Event handler for Send button click
    private async void SendMessageButton_Clicked(object sender, EventArgs e)
    {

        var token = await SecureStorage.GetAsync("auth_token");
        var user = await Shortcut.GetLoginedUser(token);




        //var message = MessageEntry.Text;
        //if (!string.IsNullOrWhiteSpace(message))
        //{
        //    try
        //    {
        //        // Send the message to the SignalR hub
        //        await _hubConnection.InvokeCoreAsync("SendMessageToAll", args: new object[] { user.FireBaseId, message });

        //        //await _hubConnection.SendAsync("SendMessage", "47813622-dd53-40da-9726-a22fc48a718a", message);
        //        MessageEntry.Text = string.Empty; // Clear the input box
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine($"Error sending message: {ex.Message}");
        //    }
        //}
    }

    protected override void OnDisappearing()
    {
        base.OnDisappearing();
        if (_hubConnection != null)
        {
            _hubConnection.StopAsync();
        }
    }

    public void Dispose()
    {
        _hubConnection?.DisposeAsync();
    }
}

public class MessageModel
{
    public string UserId { get; set; }
    public string Message { get; set; }
}