using Microsoft.AspNetCore.Mvc;
using WebRealChatAPI.Services;
using WebRealChatAPI.Models;

namespace WebRealChatAPI.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ChatController : ControllerBase
    {
        // Reference to the chat service
        private readonly IChatService _chatService;

        // Constructor to inject the chat service
        public ChatController(IChatService chatService)
        {
            _chatService = chatService;
        }

        // Endpoint to get messages between two users
        [HttpGet("messages")]
        public async Task<IActionResult> GetMessages([FromQuery] string currentUser, [FromQuery] string selectedUser)
        {
            var result = await _chatService.GetMessagesAsync(currentUser, selectedUser);
            if (result.Success)
            {
                return Ok(result.Messages); // Return messages if retrieval is successful
            }
            return StatusCode(500, result.ErrorMessage); // Return server error if retrieval fails
        }

        // Endpoint to get list of online users
        [HttpGet("onlineUsers")]
        public IActionResult GetOnlineUsers()
        {
            var onlineUsers = _chatService.GetOnlineUsers();
            return Ok(onlineUsers); // Return list of online users
        }
    }
}
