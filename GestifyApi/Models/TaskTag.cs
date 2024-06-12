namespace GestifyApi.Models
{
    public class TaskTag
    {
        public int TaskID { get; set; }
        public TaskModel Task { get; set; }  // Usando TaskModel aqui
        public int TagID { get; set; }
        public Tag Tag { get; set; }
    }
}
