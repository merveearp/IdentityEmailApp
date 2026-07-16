namespace IdentityEmailApp.Models
{
    public class MessageWithReceiverInfoModel
    {
        public int MessageId { get; set; }
        public string Subject { get; set; }
        public string MessageDetail { get; set; }
        public DateTime SendDate { get; set; }
        public string ReceiverEmail { get; set; }
        public string ReceiverName { get; set; }
        public string ReceiverSurname { get; set; }
        public string? CategoryName { get; set; }
        public bool IsRead { get; set; }


    }
}
