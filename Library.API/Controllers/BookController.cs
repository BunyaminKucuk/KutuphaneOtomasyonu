﻿using DataAccess.Abstract;
using DataAccess.Concrete;
using Entity.Concrete;
using Entity.Identity;
using Library.API.Model;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;
using System.Text.Json;

namespace Library.API.Controllers
{
    [Route("api/[controller]")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class BookController : Controller
    {
        #region Fields

        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<ApplicationUser> _userManager;
        #endregion

        #region Ctor

        public BookController(IUnitOfWork unitOfWork, UserManager<ApplicationUser> userManager)

        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
        }

        #endregion

        #region Methods

        [HttpGet("GetBookList")]
        public IList<Book> GetBookList()
        {
            List<Book> response = new List<Book>();
            try
            {
                var list = _unitOfWork.Book.GetListAll().Where(x => x.Deleted == false);
                if (list.Count() == 0)
                {
                    throw new Exception("Kitap bilgileri alınamadı.");
                }
                foreach (var item in list)
                {
                    response.Add(new Book
                    {
                        Id = item.Id,
                        BookName = item.BookName,
                        BookISBN = item.BookISBN,
                        BookPage = item.BookPage,
                        BookType = item.BookType,
                        Deleted = item.Deleted,
                        BookWriter = item.BookWriter,
                        CreatedOnUtc = item.CreatedOnUtc,
                        BookDescription = item.BookDescription,
                        BookImageUrl = item.BookImageUrl,
                        BookStatus = item.BookStatus
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
        public IActionResult AddNewBook([FromBody] Book model)
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
                book.BookStatus = false;
                book.CreatedOnUtc = DateTime.Now;
                _unitOfWork.Book.Insert(book);
                _unitOfWork.SaveChanges();
                return Ok();
            }
            catch (Exception ex)
            {

                throw ex;
            }

            return BadRequest();
        }

        [HttpGet("GetBookById")]
        public Book GetBookById(int id)
        {
            try
            {
                var book = _unitOfWork.Book.GetListAll().FirstOrDefault(x => x.Id == id);
                if (book == null)
                {
                    throw new Exception("Belirtilen id'ye sahip kitap bulunamadı.");
                }
                return book;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpPost("UpdateBook")]
        public async Task<IActionResult> UpdateBook([FromBody] Book model)
        {
            try
            {
                var check = _unitOfWork.Book.GetListAll().Where(x => x.Id == model.Id).FirstOrDefault();
                if (check == null)
                {
                    throw new Exception("Belirtilen kitap bulunamadı.");
                }
                check.Id = model.Id;
                check.BookName = model.BookName;
                check.BookISBN = model.BookISBN;
                check.BookPage = model.BookPage;
                check.BookWriter = model.BookWriter;
                check.BookType = model.BookType;
                check.BookDescription = model.BookDescription;
                check.BookImageUrl = model.BookImageUrl;
                check.Deleted = model.Deleted;
                check.BookImageUrl = model.BookImageUrl;
                check.BookDescription = model.BookDescription;
                check.BookStatus = model.BookStatus;
                _unitOfWork.Book.Update(check);
                _unitOfWork.SaveChanges();
                return Ok();
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return BadRequest();
        }

        [HttpPost("BookDelete")]
        public void BookDelete([FromBody] int bookId)
        {
            try
            {
                var checkBook = _unitOfWork.Book.GetListAll().Where(x => x.Id == bookId).FirstOrDefault();

                if (checkBook == null)
                {
                    throw new Exception("Belirtilen kitap bulunamadı.");
                }
                checkBook.Deleted = true;
                checkBook.Id = checkBook.Id;
                checkBook.BookName = checkBook.BookName;
                checkBook.BookISBN = checkBook.BookISBN;
                checkBook.BookPage = checkBook.BookPage;
                checkBook.BookWriter = checkBook.BookWriter;
                checkBook.BookType = checkBook.BookType;
                checkBook.BookDescription = checkBook.BookDescription;
                checkBook.BookImageUrl = checkBook.BookImageUrl;
                checkBook.Deleted = checkBook.Deleted;
                checkBook.BookImageUrl = checkBook.BookImageUrl;
                checkBook.BookDescription = checkBook.BookDescription;
                checkBook.BookStatus = checkBook.BookStatus;
                _unitOfWork.Book.Update(checkBook);
                _unitOfWork.SaveChanges();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpPost("AddBookToUser")]
        public async void AddBookToUser([FromBody] TakeOfBook model)
        {
            try
            {
                var userCheck = _unitOfWork.User.GetListAll().Where(x => x.Id == model.UserId).FirstOrDefault();
                var bookCheck = _unitOfWork.Book.GetListAll().Where(x => x.Id == model.BookId).FirstOrDefault();

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

                takeOfBook.UserId = model.UserId;
                takeOfBook.BookId = model.BookId;
                takeOfBook.StartOnUtc = DateTime.Now;
                takeOfBook.IsRequest = false;
                takeOfBook.BookStatus = true;
                //aga foreign key ile bağlıysa o tabloya 1 kere insert atılmaz mı ? ya bookid yanlış böyle bir book yok yada hali hazırda o book ile işlem yapılmış bi kontrol et 

                _unitOfWork.TakeOfBook.Insert(takeOfBook);
                _unitOfWork.SaveChanges();
                //dene bakam
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpGet("GetUserBooks")]
        public List<Book> GetUserBooks(int userId)
        {
            var userBooks = new List<Book>();
            var userTakes = _unitOfWork.TakeOfBook.GetListAll().Where(x => x.UserId == userId);
            foreach (var take in userTakes)
            {
                var book = _unitOfWork.Book.GetById(take.BookId);
                userBooks.Add(book);
            }
            return userBooks;
        }


        [HttpPost("DeleteUserBook")]
        public async void DeleteUserBook(TakeOfBook model)
        {
            try
            {
                var userBook = _unitOfWork.TakeOfBook.GetListAll().Where(x => x.UserId == model.UserId && x.BookId == model.BookId).FirstOrDefault();
                if (userBook == null)
                {
                    throw new Exception("Belirtilen kullanıcı kitabı bulunamadı.");
                }
                _unitOfWork.TakeOfBook.Delete(userBook);
                _unitOfWork.SaveChanges();

            }
            catch (Exception)
            {

                throw;
            }

        }

        [HttpGet("BookAllInfo")]
        public IActionResult GetBookLoans()
        {
            var loans = _unitOfWork.TakeOfBook.GetUnreturnedBookLoans()
                .Select(l => new
                {
                    UserName = l.User.UserName,
                    BookName = l.Book.BookName,
                    StartOnUtc = l.StartOnUtc,
                    BookStatus = l.BookStatus
                })
                .ToList().Where(x => x.StartOnUtc != null && x.BookStatus == true);

            return Ok(loans);
        }


        [HttpPost("TakeBook")]
        public IActionResult TakeBook([FromBody] TakeOfBook model)
        {
            TakeOfBook takeOfBook = new TakeOfBook();
            var checkBook = _unitOfWork.Book.GetListAll().Where(x => x.Id == model.BookId);
            if (checkBook == null)
            {
                throw new Exception("Belirtilen kullanıcı kitabı bulunamadı.");
            }

            takeOfBook.BookId = model.BookId;
            takeOfBook.UserId = model.UserId;
            takeOfBook.IsRequest = true;
            takeOfBook.BookStatus = false;

            _unitOfWork.TakeOfBook.Update(takeOfBook);
            _unitOfWork.SaveChanges();
            return Ok(takeOfBook);
        }

        //talep edilen kitap listesini görüntelemk için
        [HttpGet("DesiredBooks")]
        public IActionResult DesiredBooks(int userId)
        {
            var userCheck = _unitOfWork.User.GetById(userId);
            if (userCheck == null)
            {
                throw new Exception("Belirtilen kullanıcı kitabı bulunamadı.");
            }


            var desiredBooks = _unitOfWork.TakeOfBook.GetUnreturnedBookLoans()
                .Where(x => x.UserId == userCheck.Id && x.IsRequest == true)
                .Select(x => x.Book)
                .ToList();

            if (desiredBooks?.Count == 0)
            {
                throw new Exception("Belirtilen Kullanıcının kitap  isteği bulunamadı.");
            }

            var options = new JsonSerializerOptions
            {
                ReferenceHandler = ReferenceHandler.Preserve
            };
            return Ok(JsonSerializer.Serialize(desiredBooks, options));

        }



        #endregion

    }
}
