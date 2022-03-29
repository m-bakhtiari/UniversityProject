using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using UniversityProject.Core.DTOs;
using UniversityProject.Core.Repositories;
using UniversityProject.Core.Utils;
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
        public async Task<IActionResult> Create(BookDto bookDto)
        {
            ViewBag.ReturnUrl = "/Admin/Book/Create";
            ViewBag.ShowModal = "false";

            var book = new Book()
            {
                AuthorName = bookDto.AuthorName,
                Description = bookDto.Description,
                IsAvailable = bookDto.IsAvailable,
                PublishDate = new DateTime(bookDto.PublishYear, bookDto.PublishMonth, 1, new PersianCalendar()),
                UsableDays = bookDto.UsableDays,
                PublisherName = bookDto.PublisherName,
                Title = bookDto.Title,
            };
            var addBook = await _bookRepository.Insert(book, bookDto.Image);
            if (string.IsNullOrWhiteSpace(addBook) == false && Type.GetType(addBook) == typeof(string))
            {
                var category = await _categoryRepository.GetAll();
                bookDto.Categories = category;
                ViewBag.ShowModal = "true";
                return View("Create", bookDto);
            }

            foreach (var item in bookDto.CategoryIds)
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
                    ViewBag.ShowModal = "true";
                    return View("Create", bookDto);
                }
            }
            return Redirect("/Admin/Book");
        }

        public async Task<IActionResult> Delete(int id)
        {
            await _bookCategoryRepository.DeleteByBookId(id);
            await _bookRepository.Delete(id);
            return Redirect("/Admin/Book");
        }

        public async Task<IActionResult> Update(int id)
        {
            ViewBag.ReturnUrl = "/Admin/Book/Update";
            ViewBag.ShowModal = "false";
            var book = await _bookRepository.GetItem(id);
            var bookCategory = await _bookCategoryRepository.GetItemByBookId(id);
            var category = await _categoryRepository.GetAll();
            var model = new BookDto()
            {
                AuthorName = book.AuthorName,
                Description = book.Description,
                UsableDays = book.UsableDays,
                PublisherName = book.PublisherName,
                IsAvailable = book.IsAvailable,
                IsDelete = book.IsDelete,
                Title = book.Title,
                Id = book.Id,
                ImageName = book.ImageName,
                Categories = category,
                CategoryIds = bookCategory.Select(x => x.CategoryId).ToList(),
                PublishYear = int.Parse(book.PublishDate.ToShamsi().Split("/").FirstOrDefault() ?? "1401"),
                PublishMonth = int.Parse(book.PublishDate.ToShamsi().Split("/")[1] ?? "1"),
            };
            return View("Create", model);
        }

        [HttpPost]
        public async Task<IActionResult> Update(BookDto bookDto)
        {
            ViewBag.ReturnUrl = "/Admin/Book/Update";
            ViewBag.ShowModal = "false";

            var book = new Book()
            {
                AuthorName = bookDto.AuthorName,
                Description = bookDto.Description,
                IsAvailable = bookDto.IsAvailable,
                PublishDate = new DateTime(bookDto.PublishYear, bookDto.PublishMonth, 1, new PersianCalendar()),
                UsableDays = bookDto.UsableDays,
                PublisherName = bookDto.PublisherName,
                Title = bookDto.Title,
            };
            var addBook = await _bookRepository.Update(book, bookDto.Image);
            if (string.IsNullOrWhiteSpace(addBook) == false && Type.GetType(addBook) == typeof(string))
            {
                var category = await _categoryRepository.GetAll();
                bookDto.Categories = category;
                ViewBag.ShowModal = "true";
                return View("Create", bookDto);
            }

            var bookCategoryList = await _bookCategoryRepository.GetItemByBookId(bookDto.Id);
            var newIds = bookCategoryList.Select(x => x.CategoryId).Except(bookDto.CategoryIds).ToList();
            if (newIds.Any())
            {
                await _bookCategoryRepository.DeleteByBookId(bookDto.Id);
                foreach (var item in bookDto.CategoryIds)
                {
                    var bookCategory = new BookCategory()
                    {
                        BookId = bookDto.Id,
                        CategoryId = item
                    };
                    var addBookCategory = await _bookCategoryRepository.Insert(bookCategory);
                    if (string.IsNullOrWhiteSpace(addBookCategory) == false)
                    {
                        var category = await _categoryRepository.GetAll();
                        bookDto.Categories = category;
                        ViewBag.ShowModal = "true";
                        return View("Create", bookDto);
                    }
                }
            }

            return Redirect("/Admin/Book");
        }
    }
}
