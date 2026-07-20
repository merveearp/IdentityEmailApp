using IdentityEmailApp.Entities;
using IdentityEmailApp.Enums;

namespace IdentityEmailApp.Services.Abstract
{
    public interface ISystemEventService
    {
        Task CreateAsync(AppUser user, NotificationType notificationType);
       
    }
}
