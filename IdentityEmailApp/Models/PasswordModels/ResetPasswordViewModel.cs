using System.ComponentModel.DataAnnotations;

namespace IdentityEmailApp.Models.PasswordModels
{
    public class ResetPasswordViewModel
    {
        public string Password { get; set; }

        [Compare("Password", ErrorMessage = "Şifreler eşleşmiyor")]
        public string ConfirmPassword { get; set; }
    }
}
