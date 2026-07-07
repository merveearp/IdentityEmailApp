using System.ComponentModel.DataAnnotations;

namespace IdentityEmailApp.Models
{
    public class LoginUserViewModel
    {
        [Required]
        public string UsernameOrEmail { get; set; }

        [Required]
        public string Password { get; set; }

        public bool RememberMe { get; set; }
    }
}
