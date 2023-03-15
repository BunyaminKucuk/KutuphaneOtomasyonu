using Entity.Concrete;

namespace Library.API.Model
{
    public class TakeOnBookModel
    {
        public int UserId { get; set; }
        public int BookId { get; set; }
        public DateTime? StartOnUtc { get; set; }
        public DateTime? EndOnUtc { get; set; }
    }
}
