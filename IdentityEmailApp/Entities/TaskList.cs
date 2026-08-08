namespace IdentityEmailApp.Entities
{
    public class TaskList
    {
        public int TaskListId { get; set; }

        public string Name { get; set; }

        public DateTime CreatedDate { get; set; } = DateTime.Now;


        public string AppUserId { get; set; }
        public AppUser AppUser { get; set; }


        public ICollection<UserTask> UserTasks { get; set; }

    }
}
