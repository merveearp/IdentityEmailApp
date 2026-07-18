using System.ComponentModel.DataAnnotations;

namespace IdentityEmailApp.Models.PasswordModels
{
    public class ResetPasswordViewModel
    {
        public string UserId { get; set; }

        public string Token { get; set; }

        [Required(ErrorMessage = "Yeni şifre alanı zorunludur.")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required(ErrorMessage = "Şifre tekrar alanı zorunludur.")]
        [DataType(DataType.Password)]
        [Compare(nameof(Password),
            ErrorMessage = "Girdiğiniz şifreler birbiriyle eşleşmiyor.")]
        public string ConfirmPassword { get; set; }
    }
}