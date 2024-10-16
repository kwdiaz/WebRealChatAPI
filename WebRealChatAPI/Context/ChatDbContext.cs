using Microsoft.EntityFrameworkCore;
using WebRealChatAPI.Models;

namespace WebRealChatAPI.Context
{
    public class ChatDbContext : DbContext
    {
        public DbSet<ChatMessageModel> ChatMessages { get; set; }
        public DbSet<UserModel> Users { get; set; }

        public ChatDbContext(DbContextOptions<ChatDbContext> options) : base(options) { }
    }
}
