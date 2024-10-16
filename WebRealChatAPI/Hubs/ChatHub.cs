using Microsoft.AspNetCore.SignalR;
using WebRealChatAPI.Context;
using WebRealChatAPI.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Concurrent;

namespace WebRealChatAPI.Hubs
{
    public class ChatHub : Hub
    {
        private readonly ChatDbContext _context;

        // Diccionario en memoria para guardar los usuarios conectados
        // Usamos ConcurrentDictionary para manejar el acceso concurrente en un entorno multi-hilo
        private static readonly ConcurrentDictionary<string, string> _connectedUsers = new ConcurrentDictionary<string, string>();

        public ChatHub(ChatDbContext context)
        {
            _context = context;
        }

        // Método para obtener los usuarios conectados (para el controlador o para enviarlo a los clientes)
        public static List<string> GetConnectedUsers()
        {
            return _connectedUsers.Values.Distinct().ToList();
        }

        // Método para enviar mensajes en tiempo real a todos los clientes conectados
        public async Task SendMessage(string user, string message)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(message) || string.IsNullOrWhiteSpace(user))
                {
                    throw new HubException("El mensaje y el nombre de usuario no pueden estar vacíos.");
                }

                var chatMessage = new ChatMessageModel
                {
                    User = user,
                    Message = message,
                    Timestamp = DateTime.UtcNow
                };

                // Guardar el mensaje en la base de datos
                _context.ChatMessages.Add(chatMessage);
                await _context.SaveChangesAsync();

                // Enviar el mensaje a todos los clientes conectados
                await Clients.All.SendAsync("ReceiveMessage", user, message);
            }
            catch (Exception ex)
            {
                throw new HubException($"Error al enviar el mensaje: {ex.Message}");
            }
        }

        // Obtener el historial de mensajes
        public async Task<List<ChatMessageModel>> GetMessageHistory()
        {
            try
            {
                return await _context.ChatMessages.OrderBy(m => m.Timestamp).ToListAsync();
            }
            catch (Exception ex)
            {
                throw new HubException($"Error al cargar el historial de mensajes: {ex.Message}");
            }
        }

        // Método que se invoca cuando un usuario declara su nombre de usuario
        public async Task RegisterUser(string username)
        {
            var connectionId = Context.ConnectionId;

            if (!_connectedUsers.ContainsKey(connectionId))
            {
                // Añadir el usuario al diccionario
                _connectedUsers[connectionId] = username;

                // Notificar a todos que un nuevo usuario se ha conectado
                await Clients.All.SendAsync("UserConnected", username);

                // Actualizar la lista de usuarios conectados para todos los clientes
                await Clients.All.SendAsync("UpdateOnlineUsers", _connectedUsers.Values.Distinct().ToList());
            }
        }

        // Cuando un cliente se conecta
        public override async Task OnConnectedAsync()
        {
            var username = Context.GetHttpContext()?.Request.Query["username"].ToString();  // Obtenemos el username desde la query string
            if (!string.IsNullOrEmpty(username))
            {
                await RegisterUser(username);
            }

            await base.OnConnectedAsync();
        }

        // Cuando un cliente se desconecta
        public override async Task OnDisconnectedAsync(Exception exception)
        {
            var connectionId = Context.ConnectionId;

            if (_connectedUsers.TryRemove(connectionId, out var username))
            {
                // Notificar a todos que un usuario se ha desconectado
                await Clients.All.SendAsync("UserDisconnected", username);

                // Actualizar la lista de usuarios conectados para todos los clientes
                await Clients.All.SendAsync("UpdateOnlineUsers", _connectedUsers.Values.Distinct().ToList());
            }

            await base.OnDisconnectedAsync(exception);
        }
    }
}
