using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using UniversityProject.Core.DTOs;
using UniversityProject.Core.Repositories;
using UniversityProject.Data.Entities;

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

        [HttpPost]
        public async Task<IActionResult> Create(BookDto bookDto, List<int> categoryIds)
        {
            ViewBag.ReturnUrl = "/Admin/Book/Create";
            ViewBag.ShowModal = "false";

            var book = new Book()
            {
                AuthorName = bookDto.AuthorName,
                Description = bookDto.Description,
                IsAvailable = bookDto.IsAvailable,
                PublishDate = bookDto.PublishMonth + "-" + bookDto.PublishYear,
                UsableDays = bookDto.UsableDays,
                PublisherName = bookDto.PublisherName,
                Title = bookDto.Title,
            };
            var addBook = await _bookRepository.Insert(book, bookDto.Image);
            if (string.IsNullOrWhiteSpace(addBook) == false && Type.GetType(addBook) == typeof(string))
            {
                var category = await _categoryRepository.GetAll();
                bookDto.Categories = category;
                return View("Create", bookDto);
            }

            foreach (var item in categoryIds)
            {
                var bookCategory = new BookCategory()
                {
                    BookId = int.Parse(addBook ?? string.Empty),
                    CategoryId = item
                };
                var addBookCategory = await _bookCategoryRepository.Insert(bookCategory);
                if (string.IsNullOrWhiteSpace(addBookCategory) == false)
                {
                    var category = await _categoryRepository.GetAll();
                    bookDto.Categories = category;
                    return View("Create", bookDto);
                }
            }


            return View("Index");
        }
    }
}
