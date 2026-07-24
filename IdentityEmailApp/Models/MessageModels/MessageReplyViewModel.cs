namespace IdentityEmailApp.Models.MessageModels
{
    public class MessageReplyViewModel
    {
        public int ReplyMessageId { get; set; }
        public string Subject { get; set; }
        public string ReceiverEmail { get; set; }
        public string MessageDetail { get; set; }
    }
}
