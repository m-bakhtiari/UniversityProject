using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using UniversityProject.Core.Repositories;
using UniversityProject.Core.Utils;
using UniversityProject.Data.Entities;

namespace UniversityProject.WebApp.Controllers
{
    public class DashboardController : Controller
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IUserBookRepository _userBookRepository;
        private readonly IFavoriteBookRepository _favoriteBookRepository;
        private readonly IShoppingCartRepository _shoppingCartRepository;

        public DashboardController(ICategoryRepository categoryRepository, IUserBookRepository userBookRepository, IFavoriteBookRepository favoriteBookRepository, IShoppingCartRepository shoppingCartRepository)
        {
            _categoryRepository = categoryRepository;
            _userBookRepository = userBookRepository;
            _favoriteBookRepository = favoriteBookRepository;
            _shoppingCartRepository = shoppingCartRepository;
        }

        public async Task<IActionResult> Index()
        {
            ViewData["Category"] = await _categoryRepository.GetAll();
            if (User.Identity.IsAuthenticated)
            {
                ViewBag.WishListCount = await _favoriteBookRepository.CountByUserId(User.GetUserId());
                ViewBag.CartCount = await _shoppingCartRepository.CountByUserId(User.GetUserId());
            }

            return View();
        }

        public async Task<IActionResult> FinalizeCart(Book book)
        {

            return View("Index");
        }
    }
}
