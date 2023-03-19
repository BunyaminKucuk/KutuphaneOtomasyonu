using DataAccess.Abstract;
using DataAccess.Concrete;
using Entity.Concrete;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Repositories.EntityFramework
{
    public class EfTakeOfBookRepository : GenericRepository<TakeOfBook>, ITakeOfBook
    {
        public EfTakeOfBookRepository(DbContext dbContext) : base(dbContext)
        {

        }
        public LibraryContext? _context { get { return Context as LibraryContext; } }

        public IEnumerable<TakeOfBook> GetUnreturnedBookLoans()
        {
            return _context.TakeOfBooks
                .Include(l => l.Book)
                .Include(l => l.User)
                .Where(l => l.EndOnUtc == null).ToList();
        }
    }
}
