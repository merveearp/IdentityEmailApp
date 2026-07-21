using IdentityEmailApp.Context;
using IdentityEmailApp.Entities;
using IdentityEmailApp.Enums;
using IdentityEmailApp.Services.Abstract;
using Microsoft.EntityFrameworkCore;

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
                        "Şifre sıfırlama işleminiz başarıyla tamamlandı.";
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
                    notification.Title = "Başarılı Giriş";
                    notification.Detail =
                        $"Hesabınıza {DateTime.Now:dd.MM.yyyy HH:mm} tarihinde başarıyla giriş yapıldı. Bu işlem size ait değilse lütfen şifrenizi değiştirin.";
                    break;


                case NotificationType.LoginFailed:
                    notification.Title = "Başarısız Giriş Denemesi";
                    notification.Detail =
                        "Hesabınıza başarısız bir giriş denemesi gerçekleştirildi.";
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

                case NotificationType.NewMessageReceived:
                    notification.Title = "Yeni Mesajınız Var";
                    notification.Detail =
                        "Gelen kutunuza yeni bir mesaj ulaştı.";
                    break;

                case NotificationType.NewUser:
                    notification.Title = "Notika'ya Hoş Geldiniz";
                    notification.Detail =
                        "Notika'nın güçlü ve güvenilir kullanıcı deneyimine hoş geldiniz.";
                    break;

                case NotificationType.ProfileCompletionReminder:
                    notification.Title = "Profilinizi Tamamlayın";
                    notification.Detail =
                        "Eksik profil bilgilerinizi doldurarak profilinizi tamamlayabilirsiniz.";
                    break;

                default:
                    notification.Title = "Sistem Bildirimi";
                    notification.Detail =
                        "Hesabınızla ilgili yeni bir sistem bildirimi bulunuyor.";
                    break;
            }
            return notification;
        }

        public async Task CreateAsync(AppUser user,NotificationType notificationType)
        {
            if (notificationType == NotificationType.ProfileCompletionReminder)
            {
                var notificationExists = await _context.Notifications
                    .AnyAsync(x =>
                        x.AppUserId == user.Id &&
                        x.NotificationType == notificationType &&
                        x.IsRead == false);

                if (notificationExists)
                {
                    return;
                }
            }

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
