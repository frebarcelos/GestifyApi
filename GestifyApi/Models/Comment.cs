namespace GestifyApi.Models
{

    public class Comment
    {
        public int ID { get; set; }
        public int TaskID { get; set; }
        public TaskModel Task { get; set; }
        public int UserID { get; set; }
        public User User { get; set; }
        public string CommentText { get; set; }
        public DateTime CommentDate { get; set; }
    }
}