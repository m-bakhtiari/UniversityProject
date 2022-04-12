using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using UniversityProject.Core.Repositories;

namespace UniversityProject.WebApp.Controllers
{
    public class BookController : Controller
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IBookRepository _bookRepository;

        public BookController(ICategoryRepository categoryRepository, IBookRepository bookRepository)
        {
            _categoryRepository = categoryRepository;
            _bookRepository = bookRepository;
        }

        [HttpGet("/Book/{bookId}")]
        public async Task<IActionResult> Index(int bookId, int pageId = 1)
        {
            ViewData["Category"] = await _categoryRepository.GetAll();
            var model = await _bookRepository.GetBookDetails(bookId, pageId);
            return View(model);
        }
    }
}
