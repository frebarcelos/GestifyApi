namespace GestifyApi.Models
{
    public class Tag
    {
        public int ID { get; set; }
        public string TagName { get; set; }
        public ICollection<TaskTag> TaskTags { get; set; }  
    }
}
