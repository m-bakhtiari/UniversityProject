using System;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using UniversityProject.Core.Repositories;
using UniversityProject.Core.Utils;
using UniversityProject.Data.Entities;

namespace UniversityProject.WebApp.Controllers
{
    public class BookController : Controller
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IBookRepository _bookRepository;
        private readonly ICommentRepository _commentRepository;

        public BookController(ICategoryRepository categoryRepository, IBookRepository bookRepository, ICommentRepository commentRepository)
        {
            _categoryRepository = categoryRepository;
            _bookRepository = bookRepository;
            _commentRepository = commentRepository;
        }

        [HttpGet("/Book/{bookId}")]
        public async Task<IActionResult> Index(int bookId, int pageId = 1)
        {
            ViewData["Category"] = await _categoryRepository.GetAll();
            ViewBag.BookId = bookId;
            var model = await _bookRepository.GetBookDetails(bookId, pageId);
            return View(model);
        }

        [HttpGet("AddComment")]
        public async Task<IActionResult> AddComment(int bookId,int commentId,string isAnswer, string commentText)
        {
            if (string.IsNullOrWhiteSpace(commentText))
            {
                return Redirect($"/Book/{bookId}");
            }

            await _commentRepository.Insert(new Comment()
            {
                BookId = bookId,
                Text = commentText,
                RecordDate = DateTime.Now,
                UserId =1001 //User.GetUserId()
            });
            return Redirect($"/Book/{bookId}");
        }

    }
}
