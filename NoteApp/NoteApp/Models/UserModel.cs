namespace NoteApp.Models
{
    public class User
    {
        public int id { get; set; }
        public string user_name { get; set; }
        public string password_hash { get; set; }
    }

    public class UserLogin
    {
        public string Username { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }
}
