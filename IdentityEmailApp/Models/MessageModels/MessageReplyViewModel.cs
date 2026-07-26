using System.ComponentModel.DataAnnotations;

namespace IdentityEmailApp.Models.MessageModels
{
    public class MessageReplyViewModel
    {
        public int ReplyMessageId { get; set; }

        [Required(ErrorMessage = "Yanıt mesajı boş bırakılamaz.")]
        public string MessageDetail { get; set; } = null!;
    }
}