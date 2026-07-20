using IdentityEmailApp.Enums;
using Microsoft.AspNetCore.Identity;

namespace IdentityEmailApp.Entities
{
    public class AppUser:IdentityUser
    {
        public string Name { get; set; }
        public string Surname { get; set; }

        public int? ActivationCode { get; set; }

        public string? ImageUrl { get; set; }
        public string? City { get; set; }
        public Gender? Gender { get; set; }
        public DateTime? BirthDate { get; set; }

        public bool IsProfileCompleted { get; set; }
        public bool IsProfileSetupShown { get; set; }

        public ICollection<Notification> Notifications { get; set; }
    = new List<Notification>();

    }
}
