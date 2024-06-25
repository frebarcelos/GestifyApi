namespace GestifyApi.Models
{
    public class TaskModel
    {
        public int ID { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime? CompletionDate { get; set; }
        public int? StatusID { get; set; }
        public Status? Status { get; set; }
        public int? PriorityID { get; set; }
        public Priority? Priority { get; set; }
        public int UserID { get; set; }
        public User? User { get; set; }
        public int? CategoryID { get; set; }
        public Category? Category { get; set; }
        public ICollection<TaskTag>? TaskTags { get; set; }
    }
}
