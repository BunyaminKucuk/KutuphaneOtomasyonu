namespace Library.API.Model
{
    public class LoanModel
    {
        public int BookId { get; set; }
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string BookName { get; set; }
        public string BookWriter { get; set; }
        public string BookType { get; set; }
        public string BookDescription { get; set; }
        public DateTime StartOnUtc { get; set; }
    }
}
