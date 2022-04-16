using System.Threading.Tasks;
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

        public HomeController(ICategoryRepository categoryRepository, IBannerRepository bannerRepository, IBookRepository bookRepository, ISliderRepository sliderRepository, IFavoriteBookRepository favoriteBookRepository, IShoppingCartRepository shoppingCartRepository, IMessageRepository messageRepository)
        {
            _categoryRepository = categoryRepository;
            _bannerRepository = bannerRepository;
            _bookRepository = bookRepository;
            _sliderRepository = sliderRepository;
            _favoriteBookRepository = favoriteBookRepository;
            _shoppingCartRepository = shoppingCartRepository;
            _messageRepository = messageRepository;
        }

        public async Task<IActionResult> Index()
        {
            ViewData["Category"] = await _categoryRepository.GetAll();
            if (User.Identity.IsAuthenticated)
            {
                ViewBag.WishListCount = await _favoriteBookRepository.CountByUserId(User.GetUserId());
                ViewBag.CartCount = await _shoppingCartRepository.CountByUserId(User.GetUserId());
            }

            var model = new MainPageDto()
            {
                Banners = await _bannerRepository.GetAll(),
                Sliders = await _sliderRepository.GetAll(),
                FavoriteBooks = await _bookRepository.GetLatestBook(),
                LatestBooks = await _bookRepository.GetPopularBooks(),
            };
            return View(model);
        }

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

        [HttpGet("AddToShoppingCart/{id}")]
        public async Task<int> AddToShoppingCart(int id)
        {
            var shoppingCart = new ShoppingCart()
            {
                BookId = id,
                UserId = User.GetUserId()
            };
            if (await _shoppingCartRepository.IsItemExist(shoppingCart) == false)
            {
                var result = await _shoppingCartRepository.Insert(shoppingCart);
                if (string.IsNullOrWhiteSpace(result) == false)
                {
                    return -1;
                }
            }
            return await _shoppingCartRepository.CountByUserId(User.GetUserId());
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View();
        }

        [Route("/ContactUs")]
        public async Task<IActionResult> ContactUs()
        {
            ViewData["Category"] = await _categoryRepository.GetAll();
            if (User.Identity.IsAuthenticated)
            {
                ViewBag.WishListCount = await _favoriteBookRepository.CountByUserId(User.GetUserId());
                ViewBag.CartCount = await _shoppingCartRepository.CountByUserId(User.GetUserId());
            }
            return View();
        }

        [Route("/AboutUs")]
        public async Task<IActionResult> AboutUs()
        {
            ViewData["Category"] = await _categoryRepository.GetAll();
            if (User.Identity.IsAuthenticated)
            {
                ViewBag.WishListCount = await _favoriteBookRepository.CountByUserId(User.GetUserId());
                ViewBag.CartCount = await _shoppingCartRepository.CountByUserId(User.GetUserId());
            }
            return View();
        }

        [HttpPost("/InsertMessage")]
        public async Task<IActionResult> InsertMessage(Message message)
        {
            await _messageRepository.Insert(message);
            return Redirect("/AboutUs");
        }
    }
}
