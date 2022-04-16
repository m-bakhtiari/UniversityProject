using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using UniversityProject.Core.DTOs;
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
        private readonly IUserRepository _userRepository;

        public DashboardController(ICategoryRepository categoryRepository, IUserBookRepository userBookRepository, IFavoriteBookRepository favoriteBookRepository, IShoppingCartRepository shoppingCartRepository, IUserRepository userRepository)
        {
            _categoryRepository = categoryRepository;
            _userBookRepository = userBookRepository;
            _favoriteBookRepository = favoriteBookRepository;
            _shoppingCartRepository = shoppingCartRepository;
            _userRepository = userRepository;
        }

        public async Task<IActionResult> Index()
        {
            ViewData["Category"] = await _categoryRepository.GetAll();
            if (User.Identity.IsAuthenticated)
            {
                ViewBag.WishListCount = await _favoriteBookRepository.CountByUserId(User.GetUserId());
                ViewBag.CartCount = await _shoppingCartRepository.CountByUserId(User.GetUserId());
            }

            var user = await _userRepository.GetItem(User.GetUserId());
            var model = new DashboardDto()
            {
                FavoriteBooks = await _favoriteBookRepository.GetFavoriteBookByUserId(User.GetUserId()),
                ShoppingCartBooks = await _shoppingCartRepository.GetShoppingCartByUserId(User.GetUserId()),
                OldBooks = await _userBookRepository.GetItemByUserId(User.GetUserId()),
                LoginDto = new LoginDto() { Name = user.Name, Username = user.Phone }
            };
            return View(model);
        }

        public async Task<IActionResult> FinalizeCart(Book book)
        {

            return View("Index");
        }

        [HttpPost("UpdateUserInformation")]
        public async Task<IActionResult> UpdateUserInformation(LoginDto loginDto)
        {
            if (loginDto.Password != loginDto.RePassword)
            {
                return Redirect("/Dashboard");
            }
            var user = await _userRepository.GetItem(User.GetUserId());
            user.Name = loginDto.Name;
            user.Phone = loginDto.Username;
            user.Password = PasswordHelper.EncodePasswordMd5(user.Password);
            await _userRepository.Update(user);
            return Redirect("/Dashboard");
        }

        [HttpPost("/Logout")]
        public async Task<IActionResult> LogOut()
        {
            await HttpContext.SignOutAsync();
            return Redirect("/");
        }
    }
}
