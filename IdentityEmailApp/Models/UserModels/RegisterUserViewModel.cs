using System.ComponentModel.DataAnnotations;

namespace IdentityEmailApp.Models.UserModels
{
    public class RegisterUserViewModel
    {
        [Required(ErrorMessage = "İsim alanı zorunludur")]
        public string Name { get; set; }


        [Required(ErrorMessage = "Soyisim alanı zorunludur")]
        public string Surname { get; set; }


        [Required(ErrorMessage = "Kullanıcı Adı alanı zorunludur")]
        public string Username { get; set; }


        [Required(ErrorMessage = "EMail alanı zorunludur")]
        public string Email { get; set; }


        [Required(ErrorMessage = "Şifre zorunludur")]
        public string Password { get; set; }


        [Required(ErrorMessage = "Şifre tekrarı zorunludur")]
        [Compare("Password", ErrorMessage = "Şifreler eşleşmiyor")]
        public string ConfirmPassword { get; set; }
    }
}
