using WebRealChatAPI.Models;

namespace WebRealChatAPI.Services
{
    public interface IChatService
    {
        Task<ChatResultModel> GetMessagesAsync(string currentUser, string selectedUser);
        List<string> GetOnlineUsers();
        void SaveMessage(ChatMessageModel chatMessage);
    }
}