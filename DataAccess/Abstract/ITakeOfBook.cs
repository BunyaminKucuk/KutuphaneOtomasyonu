using Entity.Concrete;

namespace DataAccess.Abstract
{
    public interface ITakeOfBook : IGenericRepository<TakeOfBook>
    {
        IEnumerable<TakeOfBook> GetUnreturnedBookLoans();
        IEnumerable<TakeOfBook> GetBookStatus();
    }
}
