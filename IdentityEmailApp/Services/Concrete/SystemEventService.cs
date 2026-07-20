using IdentityEmailApp.Context;
using IdentityEmailApp.Entities;
using IdentityEmailApp.Enums;
using IdentityEmailApp.Services.Abstract;

namespace IdentityEmailApp.Services.Concrete
{
    public class SystemEventService : ISystemEventService
    {
        private readonly EmailContext _context;
        private readonly ILogger<SystemEventService> _logger;

        public SystemEventService(EmailContext context, ILogger<SystemEventService> logger)
        {
            _context = context;
            _logger = logger;
        }

        private Notification CreateNotification(string userId, NotificationType notificationType)
        {
            var notification = new Notification
            {
                AppUserId = userId,
                NotificationType = notificationType,
                IsRead = false,
                CreatedDate = DateTime.Now
            };

            switch (notificationType)
            {
                case NotificationType.AccountCreated:
                    notification.Title = "Hesabınız Oluşturuldu";
                    notification.Detail =
                        "Hesabınız başarıyla oluşturuldu.";
                    break;

                case NotificationType.EmailVerified:
                    notification.Title = "E-posta Adresiniz Doğrulandı";
                    notification.Detail =
                        "E-posta doğrulama işleminiz başarıyla tamamlandı.";
                    break;

                case NotificationType.PasswordChanged:
                    notification.Title = "Şifreniz Güncellendi";
                    notification.Detail =
                        "Hesap şifreniz başarıyla değiştirildi.";
                    break;

                case NotificationType.PasswordResetRequested:
                    notification.Title = "Şifre Sıfırlama İşlemi";
                    notification.Detail =
                        "Şifre sıfırlama talebiniz başarıyla tamamlandı.";
                    break;

                case NotificationType.ProfileUpdated:
                    notification.Title = "Profiliniz Güncellendi";
                    notification.Detail =
                        "Profil bilgileriniz başarıyla güncellendi.";
                    break;

                case NotificationType.RoleAssigned:
                    notification.Title = "Yetkileriniz Güncellendi";
                    notification.Detail =
                        "Hesabınıza ait rol ve yetki bilgileri güncellendi.";
                    break;

                case NotificationType.LoginSucceeded:
                    notification.Title = "Yeni Oturum Açıldı";
                    notification.Detail =
                        "Hesabınıza başarılı bir giriş gerçekleştirildi.";
                    break;

                case NotificationType.SecurityAlert:
                    notification.Title = "Güvenlik Bildirimi";
                    notification.Detail =
                        "Hesabınızla ilgili önemli bir güvenlik olayı tespit edildi.";
                    break;

                case NotificationType.ProfilePhotoUpdated:
                    notification.Title = "Profil Fotoğrafınız Güncellendi";
                    notification.Detail =
                        "Profil fotoğrafınız başarıyla değiştirildi.";
                    break;

                default:
                    notification.Title = "Sistem Bildirimi";
                    notification.Detail =
                        "Hesabınızla ilgili yeni bir sistem bildirimi bulunuyor.";
                    break;
            }

            return notification;
        }

        public async Task CreateAsync(AppUser user, NotificationType notificationType)
        {
            var notification = CreateNotification(user.Id,notificationType);
            _logger.LogInformation(
               "Sistem olayı oluştu. EventType: {EventType}, UserId: {UserId}",
               notificationType,
               user.Id);

            await _context.Notifications.AddAsync(notification);
            await _context.SaveChangesAsync();
        }
    }
    
}
