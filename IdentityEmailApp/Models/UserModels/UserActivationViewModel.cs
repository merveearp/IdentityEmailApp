using System.ComponentModel.DataAnnotations;

namespace IdentityEmailApp.Models.UserModels
{
    public class UserActivationViewModel
    {
        [Required(ErrorMessage = "Aktivasyon kodunu giriniz.")]
        [Range(100000, 999999,
            ErrorMessage = "Aktivasyon kodu 6 haneli olmalıdır.")]
        public int ActivationCode { get; set; }
    }
}
