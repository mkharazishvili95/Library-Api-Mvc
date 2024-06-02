namespace Library_Mvc.Models
{
    public class Book
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Image { get; set; } = string.Empty;
        public double Rating { get; set; } = 5.0f;
        public DateTime PublicationDate { get; set; }
        public bool IsCheckedOut { get; set; } = false;
        public ICollection<Author> Authors { get; set; } = new List<Author>();
    }
}
