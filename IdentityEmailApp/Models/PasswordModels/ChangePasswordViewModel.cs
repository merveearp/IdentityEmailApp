using System.ComponentModel.DataAnnotations;
using System.Globalization;

namespace IdentityEmailApp.Models.PasswordModels
{
    public class ChangePasswordViewModel
    {
        public string UserId { get; set; }

        [Required(ErrorMessage ="Mevcut Şifrenizi Giriniz")]
        [DataType(DataType.Password)]
        public string CurrentPassword { get; set; }


        [Required(ErrorMessage = "Yeni Şifrenizi Giriniz")]
        [DataType(DataType.Password)]
        public string NewPassword { get; set; }

        [Required(ErrorMessage = "Yeni Şifrenizi Tekrar Giriniz")]
        [DataType(DataType.Password)]
        [Compare(nameof(NewPassword), ErrorMessage = "Girdiğiniz yeni şifreler birbiriyle eşleşmiyor.")]
        public string ConfirmNewPassword { get; set; }
    }
}
