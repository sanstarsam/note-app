namespace NoteApp.Models
{
    public class Note
    {
        public int id { get; set; }
        public int user_id { get; set; }
        public string title { get; set; }
        public string content { get; set; }
        public DateTime created_at { get; set; }
        public DateTime updated_at { get; set; }
    }
}
