using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using UniversityProject.Core.DTOs;
using UniversityProject.Core.Repositories;
using UniversityProject.Core.Utils;

namespace UniversityProject.WebApp.Controllers
{
    public class LibraryController : Controller
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IBookRepository _bookRepository;
        private readonly IFavoriteBookRepository _favoriteBookRepository;
        private readonly IShoppingCartRepository _shoppingCartRepository;

        public LibraryController(ICategoryRepository categoryRepository, IBookRepository bookRepository, IFavoriteBookRepository favoriteBookRepository, IShoppingCartRepository shoppingCartRepository)
        {
            _categoryRepository = categoryRepository;
            _bookRepository = bookRepository;
            _favoriteBookRepository = favoriteBookRepository;
            _shoppingCartRepository = shoppingCartRepository;
        }

        [HttpGet]
        public async Task<IActionResult> Index(LibraryDto libraryDto)
        {
            await GetMenuData();

            libraryDto.StartDate = libraryDto.StartDate.ToEnglishNumbers();
            libraryDto.EndDate = libraryDto.EndDate.ToEnglishNumbers();
            libraryDto.StartPublishDate = libraryDto.StartPublishDate.ToEnglishNumbers();
            libraryDto.EndPublishDate = libraryDto.EndPublishDate.ToEnglishNumbers();
            var model = await _bookRepository.GetLibraryData(libraryDto);
            return View(model);
        }

        [HttpGet("Search")]
        public async Task<IActionResult> Search(int? categoryId, string authorName, string title, string publisherName, bool addedDate, bool popularity)
        {
            await GetMenuData();
            var data = new LibraryDto()
            {
                Authors = authorName,
                Title = title,
                Publishers = publisherName
            };
            if (categoryId.HasValue)
            {
                data.CategoryIdSearch.Add(categoryId.Value);
            }
            var model = await _bookRepository.GetLibraryData(data);
            return View("Index", model);
        }

        private async Task GetMenuData()
        {
            ViewData["Category"] = await _categoryRepository.GetAll();
            ViewBag.Title = "کتابخانه";
            if (User.Identity.IsAuthenticated)
            {
                ViewBag.WishListCount = await _favoriteBookRepository.CountByUserId(User.GetUserId());
                ViewBag.CartCount = await _shoppingCartRepository.CountByUserId(User.GetUserId());
            }
        }
    }
}
