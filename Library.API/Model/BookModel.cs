namespace Library.API.Model
{
    public class BookModel
    {
        public int Id { get; set; }
        public string? BookName { get; set; }
        public string? BookWriter { get; set; }
        public string? BookType { get; set; }
        public string? BookPage { get; set; }
        public string? BookISBN { get; set; }
        public DateTime CreatedOnUtc { get; set; }
        public string? BookDescription { get; set; }
        public string? BookImageUrl { get; set; }
        public bool Deleted { get; set; }
        /// <summary>
        /// Gets or sets a value indicating whether the entity has been deleted
        /// </summary>
        public bool BookStatus { get; set; }
    }
}
