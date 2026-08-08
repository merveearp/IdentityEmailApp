
namespace IdentityEmailApp.DTOs.TaskDtos
{
    using System.ComponentModel.DataAnnotations;

        public class CreateTaskDto
        {
            [Required(ErrorMessage = "Görev başlığı zorunludur.")]
            public string Title { get; set; } = string.Empty;

            public string? Description { get; set; }

            public DateTime? DueDate { get; set; }

            public DateTime? ReminderDate { get; set; }

            public bool IsImportant { get; set; }

            [Display(Name = "Kategori")]
            public int? TaskListId { get; set; }

            public List<CreateSubTaskDto> SubTasks { get; set; } = new();
        }
    }

