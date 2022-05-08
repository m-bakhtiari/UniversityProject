using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UniversityProject.Core.DTOs;
using UniversityProject.Core.Repositories;
using UniversityProject.Core.Utils;
using UniversityProject.Data.Entities;

namespace UniversityProject.WebApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IBannerRepository _bannerRepository;
        private readonly IBookRepository _bookRepository;
        private readonly ISliderRepository _sliderRepository;
        private readonly IFavoriteBookRepository _favoriteBookRepository;
        private readonly IShoppingCartRepository _shoppingCartRepository;
        private readonly IMessageRepository _messageRepository;
        private readonly ITeamRepository _teamRepository;
        private readonly IUserRepository _userRepository;
        private readonly ICommentRepository _commentRepository;

        public HomeController(ICategoryRepository categoryRepository, IBannerRepository bannerRepository, IBookRepository bookRepository, ISliderRepository sliderRepository, IFavoriteBookRepository favoriteBookRepository, IShoppingCartRepository shoppingCartRepository, IMessageRepository messageRepository, ITeamRepository teamRepository, IUserRepository userRepository, ICommentRepository commentRepository)
        {
            _categoryRepository = categoryRepository;
            _bannerRepository = bannerRepository;
            _bookRepository = bookRepository;
            _sliderRepository = sliderRepository;
            _favoriteBookRepository = favoriteBookRepository;
            _shoppingCartRepository = shoppingCartRepository;
            _messageRepository = messageRepository;
            _teamRepository = teamRepository;
            _userRepository = userRepository;
            _commentRepository = commentRepository;
        }

        public async Task<IActionResult> Index()
        {
            await GetMenuData();
            var model = new MainPageDto()
            {
                Banners = await _bannerRepository.GetAll(),
                Sliders = await _sliderRepository.GetAll(),
                FavoriteBooks = await _bookRepository.GetLatestBook(),
                LatestBooks = await _bookRepository.GetPopularBooks(),
            };
            ViewBag.Title = "صفحه اصلی";
            return View(model);
        }

        [Authorize]
        [HttpGet("AddToFavoriteBook/{id}")]
        public async Task<int> AddToFavoriteBook(int id)
        {
            var favoriteBook = new FavoriteBook()
            {
                BookId = id,
                UserId = User.GetUserId()
            };
            if (await _favoriteBookRepository.IsItemExist(favoriteBook) == false)
            {
                await _favoriteBookRepository.Insert(favoriteBook);
            }
            return await _favoriteBookRepository.CountByUserId(User.GetUserId());
        }

        [Authorize]
        [HttpGet("AddToShoppingCart/{id}")]
        public async Task<int> AddToShoppingCart(int id)
        {
            var shoppingCart = new ShoppingCart()
            {
                BookId = id,
                UserId = User.GetUserId()
            };
            var res = await _shoppingCartRepository.BookValidation(shoppingCart);
            if (res != 0) return res;
            await _shoppingCartRepository.Insert(shoppingCart);
            return await _shoppingCartRepository.CountByUserId(User.GetUserId());
        }

        [HttpPost("/InsertMessage")]
        public async Task<IActionResult> InsertMessage(Message message)
        {
            await _messageRepository.Insert(message);
            await GetMenuData();
            ViewBag.ShowModal = "true";
            ViewBag.Title = "تماس با ما";
            return View("ContactUs", new Message());
        }

        //[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public async Task<IActionResult> Error()
        {
            await GetMenuData();
            ViewBag.Title = "خطا";
            return View();
        }

        [Route("/ContactUs")]
        public async Task<IActionResult> ContactUs()
        {
            await GetMenuData();
            ViewBag.Title = "تماس با ما";
            return View(new Message());
        }

        [Route("/AboutUs")]
        public async Task<IActionResult> AboutUs()
        {
            await GetMenuData();
            ViewBag.Title = "درباره ی ما";
            var model = new AboutUsDto()
            {
                Teams = await _teamRepository.GetAll(),
                BookCount = await _bookRepository.BookCount(),
                CategoryCount = await _categoryRepository.CategoryCount(),
                UserCount = await _userRepository.UserCount(),
                CommentCount = await _commentRepository.CommentCount()
            };
            return View(model);
        }

        private async Task GetMenuData()
        {
            ViewData["Category"] = await _categoryRepository.GetAll();
            if (User.Identity.IsAuthenticated)
            {
                ViewBag.WishListCount = await _favoriteBookRepository.CountByUserId(User.GetUserId());
                ViewBag.CartCount = await _shoppingCartRepository.CountByUserId(User.GetUserId());
            }
        }
    }
}
