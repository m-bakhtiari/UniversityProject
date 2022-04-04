using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using UniversityProject.Core.DTOs;
using UniversityProject.Core.Repositories;
using UniversityProject.Data.Entities;

namespace UniversityProject.WebApp.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class UserBookController : Controller
    {
        private readonly IUserBookRepository _userBookRepository;
        private readonly IBookRepository _bookRepository;
        private readonly IUserRepository _userRepository;

        public UserBookController(IUserBookRepository userBookRepository, IBookRepository bookRepository, IUserRepository userRepository)
        {
            _userBookRepository = userBookRepository;
            _bookRepository = bookRepository;
            _userRepository = userRepository;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var model = await _userBookRepository.GetAll();
            return View(model);
        }

        public async Task<IActionResult> Create()
        {
            ViewBag.ReturnUrl = "/Admin/UserBook/Create";
            ViewBag.ShowModal = "false";
            var model = new UserBookDto()
            {
                BookTitles = await _bookRepository.GetAllTitles(),
                Users = await _userRepository.GetAll()
            };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Create(UserBookDto userBookDto)
        {
            ViewBag.ReturnUrl = "/Admin/UserBook/Create";
            ViewBag.ShowModal = "false";
            var res = await _userBookRepository.Insert(new UserBook()
            {
                BookId = userBookDto.BookId,
                UserId = userBookDto.UserId,
                StartDate = Convert.ToDateTime(userBookDto.StartDate),
                EndDate = Convert.ToDateTime(userBookDto.EndDate)
            });
            if (string.IsNullOrWhiteSpace(res) == false)
            {
                ViewBag.ShowModal = "true";
                userBookDto.BookTitles = await _bookRepository.GetAllTitles();
                userBookDto.Users = await _userRepository.GetAll();
                return View(userBookDto);
            }
            return Redirect("/Admin/UserBook");
        }

        public async Task<IActionResult> Update(int id)
        {
            ViewBag.ReturnUrl = "/Admin/UserBook/Update";
            ViewBag.ShowModal = "false";
            var userBook = await _userBookRepository.GetItem(id);
            var model = new UserBookDto()
            {
                UserId = userBook.UserId,
                BookId = userBook.BookId,
                StartDate = userBook.StartDate.ToString("yyyy/MM/dd"),
                EndDate = userBook.EndDate?.ToString("yyyy/MM/dd"),
                Users = await _userRepository.GetAll(),
                BookTitles = await _bookRepository.GetAllTitles(),
                Id = userBook.Id,
            };
            return View("Create", model);
        }

        [HttpPost]
        public async Task<IActionResult> Update(UserBookDto userBookDto)
        {
            await _userBookRepository.Update(new UserBook()
            {
                BookId = userBookDto.BookId,
                UserId = userBookDto.UserId,
                StartDate = Convert.ToDateTime(userBookDto.StartDate),
                EndDate = Convert.ToDateTime(userBookDto.EndDate),
                Id = userBookDto.Id
            });
            return Redirect("/Admin/UserBook");
        }

        public async Task<IActionResult> Delete(int id)
        {
            await _userBookRepository.Delete(id);
            return Redirect("/Admin/UserBook");
        }
    }
}
