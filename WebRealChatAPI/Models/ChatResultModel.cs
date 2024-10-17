namespace WebRealChatAPI.Models
{
    public class ChatResultModel
    {
        public bool Success { get; set; }
        public string ErrorMessage { get; set; }
        public List<ChatMessageModel> Messages { get; set; }
    }
}
