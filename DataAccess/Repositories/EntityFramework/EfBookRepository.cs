using DataAccess.Abstract;
using DataAccess.Concrete;
using Entity.Concrete;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Repositories.EntityFramework
{
    public class EfBookRepository : GenericRepository<Book>, IBookRepository
    {
        public EfBookRepository(DbContext dbContext) : base(dbContext)
        {

        }
        public LibraryContext? _context { get { return Context as LibraryContext; } }
    }
}
