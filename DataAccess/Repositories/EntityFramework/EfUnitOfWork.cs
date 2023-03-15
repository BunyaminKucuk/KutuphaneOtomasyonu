using DataAccess.Abstract;
using DataAccess.Concrete;

namespace DataAccess.Repositories.EntityFramework
{
    public class EfUnitOfWork : IUnitOfWork
    {
        private readonly LibraryContext _libraryContext;

        public EfUnitOfWork(LibraryContext libraryContext)
        {

            _libraryContext = libraryContext ?? throw new ArgumentNullException("dbcontext boş olamaz");
        }


        private IBookRepository _book;
        public IBookRepository Book => _book ??= new EfBookRepository(_libraryContext);

        private IUserRepository _user;
        public IUserRepository User => _user ??= new EfUserRepository(_libraryContext);

        private ITakeOfBook _takeOfBook;
        public ITakeOfBook TakeOfBook => _takeOfBook ??= new EfTakeOfBookRepository(_libraryContext);




        public void Dispose()
        {
            _libraryContext.Dispose();
        }

        public int SaveChanges()
        {
            return _libraryContext.SaveChanges();
        }
    }
}

