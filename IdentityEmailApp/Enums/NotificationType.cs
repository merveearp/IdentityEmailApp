namespace IdentityEmailApp.Enums
{
    public enum NotificationType
    {
        // Hesap
        AccountCreated = 1,
        EmailVerified = 2,
        LoginSucceeded = 3,
        LoginFailed = 4,
        PasswordChanged = 5,
        PasswordResetRequested = 6,

        // Profil
        ProfileUpdated = 7,
        ProfilePhotoUpdated = 8,
        ProfileCompletionReminder = 9,

        // Roller
        RoleAssigned = 10,

        // Mesajlar
        NewMessageReceived = 11,

        // Güvenlik
        SecurityAlert = 12,

        // Yönetim
        NewUser = 13,

        // Bilgilendirme
        WelcomeTip = 14
    }
}