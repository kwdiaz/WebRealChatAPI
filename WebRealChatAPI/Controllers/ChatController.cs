using Microsoft.AspNetCore.Mvc;
using WebRealChatAPI.Context;
using WebRealChatAPI.Models;
using Microsoft.EntityFrameworkCore;
using WebRealChatAPI.Hubs;

namespace WebRealChatAPI.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ChatController : ControllerBase
    {
        private readonly ChatDbContext _context;

        public ChatController(ChatDbContext context)
        {
            _context = context;
        }

        // Obtener el historial de mensajes
        [HttpGet("messages")]
        public async Task<IActionResult> GetMessages()
        {
            try
            {
                // Devolver mensajes ordenados por timestamp
                var messages = await _context.ChatMessages.OrderBy(m => m.Timestamp).ToListAsync();
                return Ok(messages);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error al obtener los mensajes: {ex.Message}");
            }
        }

        // Enviar un nuevo mensaje
        [HttpPost("messages")]
        public async Task<IActionResult> SendMessage([FromBody] MessageRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.User) || string.IsNullOrWhiteSpace(request.Message))
            {
                return BadRequest("El usuario y el mensaje no pueden estar vacíos.");
            }

            try
            {
                var message = new ChatMessageModel
                {
                    User = request.User,
                    Message = request.Message,
                    Timestamp = DateTime.UtcNow
                };

                // Guardar el mensaje en la base de datos
                _context.ChatMessages.Add(message);
                await _context.SaveChangesAsync();

                return Ok("Mensaje enviado correctamente.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error al enviar el mensaje: {ex.Message}");
            }
        }

        // Obtener usuarios en línea
        [HttpGet("onlineUsers")]
        public IActionResult GetOnlineUsers()
        {
            // Llamar al método estático GetConnectedUsers de ChatHub
            var onlineUsers = ChatHub.GetConnectedUsers();
            return Ok(onlineUsers);
        }
    }

    // Clase de solicitud para enviar mensajes
    public class MessageRequest
    {
        public string User { get; set; }
        public string Message { get; set; }
    }
}
