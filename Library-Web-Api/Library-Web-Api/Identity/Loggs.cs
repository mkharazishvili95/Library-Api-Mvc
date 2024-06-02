namespace Library_Web_Api.Identity
{
    public class Loggs
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string UserRole { get; set; } = string.Empty;
        public string UserName { get; set; } = string.Empty;
        public DateTime LoggDate { get; set; } = DateTime.Now;
    }
}
