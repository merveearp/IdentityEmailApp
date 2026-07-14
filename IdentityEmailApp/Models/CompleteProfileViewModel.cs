using IdentityEmailApp.Enums;
using System.ComponentModel.DataAnnotations;

namespace IdentityEmailApp.Models
{
    public class CompleteProfileViewModel
    {
        [Display(Name = "Profil Fotoğrafı")]
        public IFormFile? ProfileImage { get; set; }

        public string? ImageUrl { get; set; }

        [Display(Name = "Telefon Numarası")]
        [Phone(ErrorMessage = "Geçerli bir telefon numarası giriniz.")]
        public string? PhoneNumber { get; set; }

        [Display(Name = "Şehir")]
        public string? City { get; set; }

        [Display(Name = "Doğum Tarihi")]
        [DataType(DataType.Date)]
        public DateTime? BirthDate { get; set; }

        [Display(Name = "Cinsiyet")]
        public Gender? Gender { get; set; }
    }
}
