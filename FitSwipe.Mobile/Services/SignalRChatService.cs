using Microsoft.AspNetCore.SignalR.Client;

namespace FitSwipe.Mobile.Services
{
    public class SignalRChatService
    {
        private readonly HubConnection _hubConnection;

        public SignalRChatService()
        {
            _hubConnection = new HubConnectionBuilder()
                .WithUrl("http://localhost:5250/chathub") // URL to your SignalR hub
                .Build();
        }

        public async Task ConnectAsync()
        {
            await _hubConnection.StartAsync();
        }

        public async Task DisconnectAsync()
        {
            await _hubConnection.StopAsync();
        }

        public async Task JoinRoom(string chatRoomId, bool isGroup)
        {
            await _hubConnection.InvokeAsync("JoinRoom", chatRoomId, isGroup);
        }

        public async Task SendMessage(string chatRoomId, string userFirebaseId, string message, bool isGroup)
        {
            await _hubConnection.InvokeAsync("SendMessage", chatRoomId, userFirebaseId, message, isGroup);
        }

        public void ReceiveMessage(Action<string, string, bool> onMessageReceived)
        {
            _hubConnection.On<string, string, bool>("ReceiveMessage", (userFirebaseId, message, isGroup) =>
            {
                onMessageReceived.Invoke(userFirebaseId, message, isGroup);
            });
        }
    }
}
