namespace WebRealChatAPI.Models
{
    public class ChatMessageModel
    {
        public int Id { get; set; }
        public string User { get; set; }
        public string Message { get; set; }
        public DateTime Timestamp { get; set; }
        public string Recipient { get; set; }
    }
}
