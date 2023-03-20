using System.ComponentModel.DataAnnotations;

namespace Entity.Concrete
{
    public class TakeOfBook
    {
        [Key]
        public int Id { get; set; }
        public int UserId { get; set; }
        public int BookId { get; set; }
        public virtual User User { get; set; }
        public virtual Book Book { get; set; }
        public DateTime? StartOnUtc { get; set; }
        public DateTime? EndOnUtc { get; set; }
        public bool? IsRequest { get; set; }

    }
}
