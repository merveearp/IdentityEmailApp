using System.ComponentModel.DataAnnotations;

namespace IdentityEmailApp.DTOs.TaskDtos
{
    public class EditTaskDto
    {
        public int UserTaskId { get; set; }

        [Required(ErrorMessage = "Görev başlığı zorunludur.")]
        public string Title { get; set; } = string.Empty;

        public string? Description { get; set; }

        public DateTime? DueDate { get; set; }

        public DateTime? ReminderDate { get; set; }

        public bool IsImportant { get; set; }

        public int? TaskListId { get; set; }
    }
}