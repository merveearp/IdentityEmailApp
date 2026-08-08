namespace IdentityEmailApp.Entities
{
    public class UserTask
    {
        public int UserTaskId { get; set; }

        public string Title { get; set; }
        public string? Description { get; set; }

        public DateTime CreatedDate { get; set; } = DateTime.Now;
        public DateTime? DueDate { get; set; }
        public DateTime? ReminderDate { get; set; }
        public DateTime? CompletedDate { get; set; }

        public bool IsCompleted { get; set; }
        public bool IsImportant { get; set; }
        public bool IsDeleted { get; set; }

        public string AppUserId { get; set; }
        public AppUser AppUser { get; set; }

     
        public int? TaskListId { get; set; }
        public TaskList? TaskList { get; set; }

       
        public ICollection<SubTask> SubTasks { get; set; }
            = new List<SubTask>();
    }
}