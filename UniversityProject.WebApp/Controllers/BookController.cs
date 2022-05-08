using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
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
        private readonly IFavoriteBookRepository _favoriteBookRepository;
        private readonly IShoppingCartRepository _shoppingCartRepository;

        public BookController(ICategoryRepository categoryRepository, IBookRepository bookRepository, ICommentRepository commentRepository, IFavoriteBookRepository favoriteBookRepository, IShoppingCartRepository shoppingCartRepository)
        {
            _categoryRepository = categoryRepository;
            _bookRepository = bookRepository;
            _commentRepository = commentRepository;
            _favoriteBookRepository = favoriteBookRepository;
            _shoppingCartRepository = shoppingCartRepository;
        }

        [HttpGet("/Book/{bookId}")]
        public async Task<IActionResult> Index(int bookId, int pageId = 1)
        {
            ViewData["Category"] = await _categoryRepository.GetAll();
            if (User.Identity.IsAuthenticated)
            {
                ViewBag.WishListCount = await _favoriteBookRepository.CountByUserId(User.GetUserId());
                ViewBag.CartCount = await _shoppingCartRepository.CountByUserId(User.GetUserId());
            }
            ViewBag.BookId = bookId;
            var model = await _bookRepository.GetBookDetails(bookId, pageId);
            ViewBag.ShowComment = "false";
            ViewBag.PageId = pageId;
            ViewBag.Title = "مشخصات کتاب";
            return View(model);
        }

        [Authorize]
        [HttpPost("AddComment")]
        public async Task<IActionResult> AddComment(int bookId, int? commentId, string isAnswer, string commentText, int pageId = 1)
        {
            if (string.IsNullOrWhiteSpace(commentText))
            {
                return Redirect($"Book/{bookId}?PageId={pageId}#reviews");
            }

            if (isAnswer == "true")
            {
                await _commentRepository.Insert(new Comment()
                {
                    ParentId = commentId,
                    BookId = bookId,
                    Text = commentText,
                    RecordDate = DateTime.Now,
                    UserId = User.GetUserId()
                });
            }
            else if (commentId == null)
            {
                await _commentRepository.Insert(new Comment()
                {
                    BookId = bookId,
                    Text = commentText,
                    RecordDate = DateTime.Now,
                    UserId = User.GetUserId()
                });
            }
            else
            {
                await _commentRepository.Update(new Comment()
                {
                    Id = commentId.Value,
                    Text = commentText,
                });
            }

            return Redirect($"Book/{bookId}?PageId={pageId}#reviews");
        }
    }
}
