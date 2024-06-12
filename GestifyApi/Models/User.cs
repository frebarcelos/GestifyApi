using System.ComponentModel.DataAnnotations.Schema;

namespace GestifyApi.Models
{
    public class User
    {
        public int ID { get; set; }
        public string Username { get; set; }
        public string PasswordHash { get; set; }
    }

    public class RegisterRequest
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }

    public class LoginRequest
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }
}