﻿using Entity.Concrete;
using Library.API.Model;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Security.Claims;

namespace LibraryUI.Controllers
{
    [Authorize(Policy = "LibraryPolicy")]
    public class BookController : BaseController
    {
        private readonly HttpClient _httpClient = new HttpClient();

        //public async Task<IActionResult> BookReadMore(int id)
        //{
        //    var token = HttpContext.User.Claims.Where(x => x.Type == ClaimTypes.Authentication).FirstOrDefault().Value;
        //    _httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);
        //    var responseMessage = await _httpClient.GetAsync("https://localhost:7299/api/Book/GetBookById?bookId=" + id);
        //    var jsonString = await responseMessage.Content.ReadAsStringAsync();
        //    var values = JsonConvert.DeserializeObject<List<Book>>(jsonString);
        //    return View(values);
        //}
        [HttpGet]
        public async Task<IActionResult> AllInfoTakeBook()
        {
            var token = HttpContext.User.Claims.Where(x => x.Type == ClaimTypes.Authentication).FirstOrDefault().Value;
            _httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);
            var responseMessage = await _httpClient.GetAsync("https://localhost:7299/api/Book/BookAllInfo");

            if (responseMessage.IsSuccessStatusCode)
            {
                var jsonString = await responseMessage.Content.ReadAsStringAsync();
                var loans = JsonConvert.DeserializeObject<List<LoanModel>>(jsonString);
                // Verileri kullanarak işlemlerinizi yapabilirsiniz
                return View(loans);
            }

            return View();
        }

        [HttpGet]
        public async Task<IActionResult> TakeOnBook()
        {
            var token = HttpContext.User.Claims.Where(x => x.Type == ClaimTypes.Authentication).FirstOrDefault().Value;
            _httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);
            var responseMessage = await _httpClient.GetAsync("https://localhost:7299/api/User/GetAllUsers");
            if (responseMessage.IsSuccessStatusCode)
            {
                var jsonString = await responseMessage.Content.ReadAsStringAsync();
                var users = JsonConvert.DeserializeObject<List<User>>(jsonString);

                var model = new TakeOnBookModel
                {
                    Users = users
                };

                return View(model);
            }

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> TakeOnBook(TakeOnBookModel model, int id)
        {
            model.BookId = id;
            var token = HttpContext.User.Claims.Where(x => x.Type == ClaimTypes.Authentication).FirstOrDefault().Value;
            _httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);
            var responseMessage = await _httpClient.PostAsJsonAsync(new Uri("https://localhost:7299/api/Book/AddBookToUser"), model);
            if (responseMessage.IsSuccessStatusCode)
            {
                return RedirectToAction("BookList", "Book");
            }
            return View();
        }

        public async Task<IActionResult> BookList()
        {
            var token = HttpContext.User.Claims.Where(x => x.Type == ClaimTypes.Authentication).FirstOrDefault().Value;
            _httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);
            var responseMessage = await _httpClient.GetAsync("https://localhost:7299/api/Book/GetBookList");
            if (responseMessage.IsSuccessStatusCode)
            {
                var jsonString = await responseMessage.Content.ReadAsStringAsync();
                var values = JsonConvert.DeserializeObject<List<Book>>(jsonString);
                return View(values);
            }

            return View();
        }

        [HttpGet]
        public async Task<IActionResult> AddBook()
        {
            return View(new Book());
        }

        [HttpPost]
        public async Task<IActionResult> AddBook(Book model)
        {
            var token = HttpContext.User.Claims.Where(x => x.Type == ClaimTypes.Authentication).FirstOrDefault().Value;
            _httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);
            var responseMessage = await _httpClient.PostAsJsonAsync(new Uri("https://localhost:7299/api/Book/AddNewBook"), model);
            if (responseMessage.IsSuccessStatusCode)
            {
                return RedirectToAction("BookList", "Book");
            }
            return View(new Book());
        }

        [HttpGet]
        public async Task<IActionResult> BookEdit(int id)
        {
            var token = HttpContext.User.Claims.Where(x => x.Type == ClaimTypes.Authentication).FirstOrDefault().Value;
            _httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);
            var responseMessage = await _httpClient.GetAsync("https://localhost:7299/api/Book/GetBookById?id=" + id);
            if (responseMessage.IsSuccessStatusCode)
            {
                var jsonString = await responseMessage.Content.ReadAsStringAsync();
                var model = JsonConvert.DeserializeObject<Book>(jsonString);
                return View(model);
            }

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> BookEdit(Book model)
        {
            var token = HttpContext.User.Claims.Where(x => x.Type == ClaimTypes.Authentication).FirstOrDefault().Value;
            _httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);
            var responseMessage = await _httpClient.PostAsJsonAsync(new Uri("https://localhost:7299/api/Book/UpdateBook"), model);
            if (responseMessage.IsSuccessStatusCode)
            {
                return RedirectToAction("BookList", "Book");
            }
            return View(new Book());
        }

        [HttpPost]
        public async Task<IActionResult> BookDelete(int id)
        {

            var token = HttpContext.User.Claims.Where(x => x.Type == ClaimTypes.Authentication).FirstOrDefault().Value;
            _httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);
            var responseMessage = await _httpClient.PostAsJsonAsync(new Uri("https://localhost:7299/api/Book/BookDelete"), id);

            if (responseMessage.IsSuccessStatusCode)
            {
                return RedirectToAction("BookList", "Book");
            }

            return Ok();

        }
    }
}


