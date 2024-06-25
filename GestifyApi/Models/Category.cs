namespace GestifyApi.Models
{
    public class Category
    {
        public int ID { get; set; }
        public string CategoryName { get; set; }
        public int UserID { get; set; }
        public User? User { get; set; }
    }
}
