using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using UniversityProject.Core.Repositories;

namespace UniversityProject.WebApp.Controllers
{
    public class BookController : Controller
    {
        private readonly ICategoryRepository _categoryRepository;

        public BookController(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        public async Task<IActionResult> Index()
        {
            ViewData["Category"] = await _categoryRepository.GetAll();

            return View();
        }
    }
}
