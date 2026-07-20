namespace IdentityEmailApp.Models.MessageModels
{
    public class NavbarMessageViewModel
    {
        public int MessageId { get; set; }
        public string SenderName { get; set; }
        public string SenderSurname { get; set; }
        public string Subject { get; set; }
        public string MessageDetail { get; set; }
        public DateTime SendDate { get; set; }
        public string? SenderImageUrl { get; set; }
        public string SenderEmail { get; set; }
    }
}
