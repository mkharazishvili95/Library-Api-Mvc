namespace Library_Web_Api.Models
{
    public class Author
    {
        public int Id { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public DateTime YearOfBirth { get; set; }
        public ICollection<Book> Books { get; set; } = new List<Book>();
    }
}
