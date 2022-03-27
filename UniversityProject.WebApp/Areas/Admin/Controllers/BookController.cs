using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using UniversityProject.Core.DTOs;
using UniversityProject.Core.Repositories;

namespace UniversityProject.WebApp.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class BookController : Controller
    {
        private readonly IBookRepository _bookRepository;
        private readonly IBookCategoryRepository _bookCategoryRepository;
        private readonly ICategoryRepository _categoryRepository;

        public BookController(IBookRepository bookRepository, IBookCategoryRepository bookCategoryRepository, ICategoryRepository categoryRepository)
        {
            _bookRepository = bookRepository;
            _bookCategoryRepository = bookCategoryRepository;
            _categoryRepository = categoryRepository;
        }

        public async Task<IActionResult> Index()
        {
            var model = await _bookRepository.GetAll();
            return View(model);
        }

        public async Task<IActionResult> Create()
        {
            ViewBag.ReturnUrl = "/Admin/Book/Create";
            ViewBag.ShowModal = "false";
            var category = await _categoryRepository.GetAll();
            var model = new BookDto()
            {
                Categories = category
            };
            return View(model);
        }
    }
}
