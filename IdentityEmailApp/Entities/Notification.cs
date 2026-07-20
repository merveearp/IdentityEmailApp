using IdentityEmailApp.Enums;

namespace IdentityEmailApp.Entities
{
    public class Notification
    {
        public int NotificationId { get; set; }

        public string Title { get; set; } = null!;

        public string Detail { get; set; } = null!;

        public NotificationType NotificationType { get; set; }

        public bool IsRead { get; set; }

        public DateTime CreatedDate { get; set; } = DateTime.Now;

        public string AppUserId { get; set; }

        public AppUser AppUser { get; set; } = null!;
    }
}