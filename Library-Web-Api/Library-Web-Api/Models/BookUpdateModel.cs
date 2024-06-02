namespace Library_Web_Api.Models
{
    public class BookUpdateModel
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string Image { get; set; }
        public double Rating { get; set; }
        public DateTime PublicationDate { get; set; }
        public bool IsCheckedOut { get; set; } = false;
        public ICollection<Author> Authors { get; set; }
    }
}
