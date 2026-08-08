namespace IdentityEmailApp.Entities
{
    public class SubTask
    {
        public int SubTaskId { get; set; }

        public string Title { get; set; }

        public bool IsCompleted { get; set; }

        public DateTime CreatedDate { get; set; } = DateTime.Now;
        public DateTime? CompletedDate { get; set; }

        public int UserTaskId { get; set; }
        public UserTask UserTask { get; set; }
    }
}
