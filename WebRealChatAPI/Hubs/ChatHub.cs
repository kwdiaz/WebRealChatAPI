using Microsoft.AspNetCore.SignalR;
using WebRealChatAPI.Models;
using WebRealChatAPI.Services;
using System.Collections.Concurrent;

namespace WebRealChatAPI.Hubs
{
    public class ChatHub : Hub
    {
        private readonly IChatService _chatService;
        // Dictionary to keep track of connected users
        private static readonly ConcurrentDictionary<string, string> _connectedUsers = new ConcurrentDictionary<string, string>();

        // Constructor to inject the chat service
        public ChatHub(IChatService chatService)
        {
            _chatService = chatService;
        }

        // Method to send a message from sender to recipient
        public async Task SendMessage(string senderUser, string recipientUser, string message)
        {
            if (string.IsNullOrWhiteSpace(message) || string.IsNullOrWhiteSpace(recipientUser) || string.IsNullOrWhiteSpace(senderUser))
            {
                throw new HubException("Message, recipient, and username cannot be empty.");
            }

            // Create chat message object
            var chatMessage = new ChatMessageModel
            {
                User = senderUser,
                Message = message,
                Timestamp = DateTime.UtcNow,
                Recipient = recipientUser
            };

            try
            {
                _chatService.SaveMessage(chatMessage); // Save message using chat service
            }
            catch (Exception ex)
            {
                throw new Exception($"Error saving the message: {ex.Message}");
            }

            // Send the message to the recipient if they are connected
            var recipientConnectionId = _connectedUsers.FirstOrDefault(x => x.Value == recipientUser).Key;
            if (recipientConnectionId != null)
            {
                await Clients.Client(recipientConnectionId).SendAsync("ReceiveMessage", senderUser, recipientUser, message);
            }

            // Send the message back to the sender
            await Clients.Caller.SendAsync("ReceiveMessage", senderUser, recipientUser, message);
        }

        // Method called when a client connects
        public override async Task OnConnectedAsync()
        {
            var username = Context.GetHttpContext()?.Request.Query["username"].ToString();
            if (!string.IsNullOrEmpty(username))
            {
                await RegisterUser(username); // Register the connected user
            }
            await base.OnConnectedAsync();
        }

        // Register a user and notify others of the connection
        public async Task RegisterUser(string username)
        {
            var connectionId = Context.ConnectionId;
            if (!_connectedUsers.ContainsKey(connectionId))
            {
                _connectedUsers[connectionId] = username;
                await Clients.All.SendAsync("UserConnected", username);
                await Clients.All.SendAsync("UpdateOnlineUsers", _connectedUsers.Values.Distinct().ToList()); // Update the list of online users
            }
        }

        // Method called when a client disconnects
        public override async Task OnDisconnectedAsync(Exception exception)
        {
            var connectionId = Context.ConnectionId;
            if (_connectedUsers.TryRemove(connectionId, out var username))
            {
                await Clients.All.SendAsync("UserDisconnected", username);
                await Clients.All.SendAsync("UpdateOnlineUsers", _connectedUsers.Values.Distinct().ToList()); // Update the list of online users
            }
            await base.OnDisconnectedAsync(exception);
        }
    }
}
