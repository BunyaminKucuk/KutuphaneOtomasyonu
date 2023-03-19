using System.ComponentModel.DataAnnotations;

namespace Entity.Concrete
{
    public class Book
    {
        [Key]
        public int Id { get; set; }
        public string? BookName { get; set; }
        public string? BookWriter { get; set; }
        public string? BookType { get; set; }
        public string? BookPage { get; set; }
        public string? BookISBN { get; set; }
        public string? BookDescription { get; set; }
        public string? BookImageUrl { get; set; }
        public DateTime CreatedOnUtc { get; set; }
        public bool Deleted { get; set; }
        //public bool BookStatus { get; set; }
        public ICollection<TakeOfBook> TakeOfBooks { get; set; }

    }
}
