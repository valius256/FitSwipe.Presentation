using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using FitSwipe.Mobile.Services;
using FitSwipe.Shared.Dtos;
using System.Collections.ObjectModel;

namespace FitSwipe.Mobile.ViewModels
{
    public partial class ChatViewModel : ObservableObject
    {
        private readonly SignalRChatService _chatService;

        // Properties to hold parameters
        public string ChatRoomId { get; set; } // Set this when you join a chat room
        public string UserFirebaseId { get; set; } // Set this when the user logs in
        public bool IsGroup { get; set; } // Set this based on chat room type

        public ObservableCollection<string> Messages { get; } = new ObservableCollection<string>();

        public ChatViewModel(SignalRChatService chatService)
        {
            _chatService = chatService;
            _chatService.ReceiveMessage(OnMessageReceived);
        }

        [RelayCommand]
        public async Task SendMessage(SendMessageParameter parameter)
        {
            await _chatService.SendMessage(parameter.ChatRoomId, parameter.UserFirebaseId, parameter.Message, parameter.IsGroup);
        }

        private void OnMessageReceived(string userFirebaseId, string message, bool isGroup)
        {
            Messages.Add($"{userFirebaseId}: {message}");
        }

        public async Task DisconnectFromChat()
        {
            await _chatService.DisconnectAsync();
        }
    }
}
