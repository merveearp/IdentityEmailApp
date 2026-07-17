using IdentityEmailApp.Enums;

namespace IdentityEmailApp.Models.UserModels
{
    public class EditUserViewModel
    {

        public string? UserName { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string? City { get; set; }
        public string? ImageUrl { get; set; }
        public string? PhoneNumber { get; set; }
        public string Email { get; set; }
        public DateTime? BirthDate { get; set; }
        public Gender? Gender { get; set; }
        public IFormFile? ProfileImage { get; set; }
    }
}
