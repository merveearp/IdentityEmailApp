using System.ComponentModel.DataAnnotations;

namespace IdentityEmailApp.DTOs.TaskDtos
{
    public class CreateSubTaskDto
    {
        [Required(ErrorMessage = "Alt görev başlığı zorunludur.")]
        public string Title { get; set; } = string.Empty;

        public DateTime? DueDate { get; set; }
    }
}