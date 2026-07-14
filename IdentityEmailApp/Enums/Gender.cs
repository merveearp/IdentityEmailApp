using System.ComponentModel.DataAnnotations;

namespace IdentityEmailApp.Enums
{
    public enum Gender
    {
        [Display(Name ="Kadın")]
        Female=1,

        [Display(Name = "Erkek")]
        Male = 2,

        [Display(Name = "Belirtmek İstemiyorum")]
        PreferNotToSay = 3

    }
}
