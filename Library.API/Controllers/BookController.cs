using DataAccess.Abstract;
using DataAccess.Repositories.EntityFramework;
using Entity.Concrete;
using Entity.Identity;
using Library.API.Model;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Library.API.Controllers
{
    [Route("api/[controller]")]
    public class BookController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<ApplicationRole> _roleManager;
        public BookController(IUnitOfWork unitOfWork, UserManager<ApplicationUser> userManager, RoleManager<ApplicationRole> roleManager)
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        [Authorize]
        [HttpGet("GetBookList")]
        public IList<BookModel> GetBookList()
        {
            List<BookModel> response = new List<BookModel>();
            try
            {
                var list = _unitOfWork.Book.GetListAll();
                if (list.Count() == 0)
                {
                    throw new Exception("Personel bilgileri alınamadı.");
                }
                foreach (var item in list)
                {
                    response.Add(new BookModel
                    {
                        Id = item.Id,
                        BookName = item.BookName,
                        BookISBN = item.BookISBN,
                        BookPage = item.BookPage,
                        BookType = item.BookType,
                        BookStatus = item.BookStatus,
                        Deleted = item.Deleted,
                        BookWriter = item.BookWriter,
                        CreatedOnUtc = item.CreatedOnUtc,
                        BookDescription = item.BookDescription,
                        BookImageUrl = item.BookImageUrl

                    });
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
            return response;
        }

        [HttpPost("AddNewBook")]
        public void AddNewBook(BookModel model)
        {
            Book book = new Book();
            try
            {
                book.BookName = model.BookName;
                book.BookISBN = model.BookISBN;
                book.BookPage = model.BookPage;
                book.BookWriter = model.BookWriter;
                book.BookType = model.BookType;
                book.BookDescription = model.BookDescription;
                book.BookImageUrl = model.BookImageUrl;
                book.Deleted = false;
                book.CreatedOnUtc = DateTime.Now;
                _unitOfWork.Book.Insert(book);
                _unitOfWork.SaveChanges();
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        [HttpGet("GetBookById")]
        public List<Book> GetBookByName(int bookId)
        {
            Book dp = new Book();
            try
            {
                var check = _unitOfWork.Book.GetListAll().Where(x => x.Id == bookId).FirstOrDefault();
                if (check == null)
                {
                    throw new Exception("Belirtilen isimde kitap bulunamadı.");
                }
                return _unitOfWork.Book.GetListAll().Where(x => x.Id == check.Id).ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpPatch("UpdateBook")]
        public void UpdateBook(BookModel model, int bookId)
        {
            try
            {
                var check = _unitOfWork.Book.GetListAll().Where(x => x.Id == bookId).FirstOrDefault();
                if (check == null)
                {
                    throw new Exception("Belirtilen kitap bulunamadı.");
                }
                check.Id = bookId;
                check.BookName = model.BookName;
                check.BookISBN = model.BookISBN;
                check.BookPage = model.BookPage;
                check.BookWriter = model.BookWriter;
                check.BookType = model.BookType;
                check.BookDescription = model.BookDescription;
                check.BookImageUrl = model.BookImageUrl;
                check.Deleted = model.Deleted;
                check.BookStatus = model.BookStatus;
                check.BookImageUrl = model.BookImageUrl;
                check.BookDescription = model.BookDescription;


                _unitOfWork.Book.Update(check);
                _unitOfWork.SaveChanges();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpPost("DeleteBook")]
        public void DeleteBook(int bookId)
        {
            try
            {
                var check = _unitOfWork.Book.GetById(bookId);
                if (check == null)
                {
                    throw new Exception("Belirtilen kitap bulunamadı.");
                }
                check.Deleted = true;
                _unitOfWork.Book.Update(check);
                _unitOfWork.SaveChanges();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpPost("AddBookToUser")]
        public async void AddBookToUser(int bookId, int userId)
        {
            try
            {
                //var userGuid = HttpContext.User.Claims.Where(x => x.Type == "UserGuid").FirstOrDefault();
                //var user  = unitOfWork.User.GetListAll().Where(x => x.Id == userGuid.Value).FirstOrDefault();
                var userCheck = _unitOfWork.User.GetListAll().Where(x => x.Id == userId).FirstOrDefault();
                var bookCheck = _unitOfWork.Book.GetListAll().Where(x => x.Id == bookId).FirstOrDefault();

                if (userCheck == null)
                {
                    throw new Exception("Kullanıcı bulunamadı.");
                }
                if (Guid.Empty == userCheck.IdentityId)
                {
                    throw new Exception("Identity bilgisi alınamadı.");
                }
                var identityUserCheck = _userManager.Users.Where(x => x.IdentityId == userCheck.IdentityId).FirstOrDefault();
                if (identityUserCheck == null)
                {
                    throw new Exception("Identity kullanıcısı bulunamadı");
                }


                TakeOfBook takeOfBook = new TakeOfBook();

                takeOfBook.UserId = userId;
                takeOfBook.BookId = bookId;
                takeOfBook.StartOnUtc=DateTime.Now;

                _unitOfWork.TakeOfBook.Insert(takeOfBook);
                _unitOfWork.SaveChanges();
                //kitaplar bookstatus=update
                //insert takeofbook
                //

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
