using DataAccess.Abstract;
using Entity.Concrete;

namespace Business.Concrete
{
    public class BookManager : IBookRepository
    {
        private readonly IBookRepository _bookRepository;

        public BookManager(IBookRepository bookRepository)
        {
            _bookRepository = bookRepository;
        }

        public void Insert(Book t)
        {
            _bookRepository.Insert(t);
        }

        public void Delete(Book t)
        {
            _bookRepository.Delete(t);
        }

        public void Update(Book t)
        {
            _bookRepository.Update(t);
        }

        public List<Book> GetListAll()
        {
            return _bookRepository.GetListAll();
        }

        public Book GetById(int id)
        {
            return _bookRepository.GetById(id);
        }

    }
}
