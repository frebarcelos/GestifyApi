namespace GestifyApi.Models
{
    public class TaskTag
    {
        public int TaskID { get; set; }
        public TaskModel Task { get; set; }
        public int TagID { get; set; }
        public Tag Tag { get; set; }
    }
}