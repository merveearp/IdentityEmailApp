using System.ComponentModel.DataAnnotations;

namespace IdentityEmailApp.DTOs.TaskDtos
{
    public class EditTaskListDto
    {
        public int TaskListId { get; set; }

        [Required(ErrorMessage = "Kategori adı zorunludur.")]
        [StringLength(
            50,
            ErrorMessage = "Kategori adı en fazla 50 karakter olabilir.")]
        public string Name { get; set; } = string.Empty;
    }
}