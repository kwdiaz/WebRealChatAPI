using Microsoft.EntityFrameworkCore;
using WebRealChatAPI.Context;
using WebRealChatAPI.Models;
using System.Collections.Concurrent;

namespace WebRealChatAPI.Services
{
    public class ChatService : IChatService
    {
        private readonly ChatDbContext _context;
        // Dictionary to keep track of connected users
        private static readonly ConcurrentDictionary<string, string> _connectedUsers = new ConcurrentDictionary<string, string>();

        // Constructor to inject the database context
        public ChatService(ChatDbContext context)
        {
            _context = context;
        }

        // Method to get messages between two users
        public async Task<ChatResultModel> GetMessagesAsync(string currentUser, string selectedUser)
        {
            try
            {
                // Query messages between current user and selected user, ordered by time
                var messages = await _context.ChatMessages
                    .Where(m => (m.User == currentUser && m.Recipient == selectedUser) || (m.User == selectedUser && m.Recipient == currentUser))
                    .OrderBy(m => m.Timestamp)
                    .ToListAsync();

                return new ChatResultModel { Success = true, Messages = messages };
            }
            catch (Exception ex)
            {
                return new ChatResultModel { Success = false, ErrorMessage = $"Error getting messages: {ex.Message}" };
            }
        }

        // Method to get list of online users
        public List<string> GetOnlineUsers()
        {
            return _connectedUsers.Values.Distinct().ToList(); // Return distinct list of online users
        }

        // Method to save a chat message to the database
        public void SaveMessage(ChatMessageModel chatMessage)
        {
            _context.ChatMessages.Add(chatMessage);
            _context.SaveChanges(); // Save message in the database
        }
    }
}
