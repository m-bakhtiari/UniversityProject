using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UniversityProject.Core.DTOs;
using UniversityProject.Core.Repositories;
using UniversityProject.Core.Utils;
using UniversityProject.Data.Entities;

namespace UniversityProject.WebApp.Controllers
{
    [Authorize]
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

        #region User Information

        public async Task<IActionResult> Index()
        {
            var user = await _userRepository.GetItem(User.GetUserId());
            var model = new DashboardDto()
            {
                LoginDto = new LoginDto() { Name = user.Name, Username = user.Phone },
            };
            await GetMenuData();
            ViewBag.Title = "داشبرد";
            return View(model);
        }

        [HttpPost("/UpdateUserInformation")]
        public async Task<IActionResult> UpdateUserInformation(LoginDto loginDto)
        {
            var user = await _userRepository.GetItem(User.GetUserId());
            var model = new DashboardDto()
            {
                LoginDto = new LoginDto() { Name = user.Name, Username = user.Phone },
            };
            await GetMenuData();
            if (loginDto.Password != loginDto.RePassword)
            {
                ViewBag.ErrorModal = "true";
                ViewBag.ModalText = "رمز عبور و تکرار آن یکسان نمی باشد";
                return View("Index", model);
            }
            user.Name = loginDto.Name;
            user.Phone = loginDto.Username;
            user.Password = PasswordHelper.EncodePasswordMd5(user.Password);
            await _userRepository.Update(user);

            ViewBag.InfoModal = "true";
            ViewBag.ModalText = "اطلاعات با موفقیت تغییر کرد";

            return View("Index", model);
        }

        #endregion

        #region Favorite Book

        [Route("FavoriteBook")]
        public async Task<IActionResult> FavoriteBook()
        {
            var model = new DashboardDto()
            {
                FavoriteBooks = await _favoriteBookRepository.GetFavoriteBookByUserId(User.GetUserId()),
            };
            await GetMenuData();
            ViewBag.Title = "داشبرد";
            return View(model);
        }

        [Route("/DeleteFavorite")]
        public async Task<IActionResult> DeleteFavorite()
        {
            await _favoriteBookRepository.DeleteByUserId(User.GetUserId());
            var model = new DashboardDto()
            {
                FavoriteBooks = await _favoriteBookRepository.GetFavoriteBookByUserId(User.GetUserId()),
            };
            await GetMenuData();
            ViewBag.InfoModal = "true";
            ViewBag.ModalText = "لیست علاقه مندی شما به روز رسانی شد";
            return View("FavoriteBook", model);
        }

        [Route("/DeleteFavorite/{bookId}")]
        public async Task<IActionResult> DeleteFavorite(int bookId)
        {
            await _favoriteBookRepository.Delete(bookId, User.GetUserId());
            ViewBag.InfoModal = "true";
            ViewBag.ModalText = "لیست علاقه مندی شما به روز رسانی شد";
            var model = new DashboardDto()
            {
                FavoriteBooks = await _favoriteBookRepository.GetFavoriteBookByUserId(User.GetUserId()),
            };
            await GetMenuData();
            return View("FavoriteBook", model);
        }

        #endregion

        #region Shopping Cart

        [Route("ShoppingCart")]
        public async Task<IActionResult> ShoppingCart()
        {
            var model = new DashboardDto()
            {
                ShoppingCartBooks = await _shoppingCartRepository.GetShoppingCartByUserId(User.GetUserId()),
            };
            await GetMenuData();
            ViewBag.Title = "داشبرد";
            return View(model);
        }

        [Route("/FinalizeCart")]
        public async Task<IActionResult> FinalizeCart()
        {
            var model = new DashboardDto()
            {
                ShoppingCartBooks = await _shoppingCartRepository.GetShoppingCartByUserId(User.GetUserId()),
            };
            await GetMenuData();
            var cart = await _shoppingCartRepository.GetShoppingCartByUserId(User.GetUserId());
            if (cart.Any(x => x.IsAvailable == false))
            {
                ViewBag.ShowModal = "true";
                ViewBag.ModalText = "بعضی از کتاب ها در دسترس نیستند ، لطفا لیست خرید را به روز رسانی نمایید";
                return View("ShoppingCart", model);
            }
            var userBook = new List<UserBook>();
            foreach (var book in cart.Where(x => x.IsAvailable).Select(x => x.Book))
            {
                userBook.Add(new UserBook() { BookId = book.Id, StartDate = DateTime.Now, UserId = User.GetUserId() });
            }
            await _userBookRepository.InsertList(userBook);
            await _shoppingCartRepository.DeleteByUserId(User.GetUserId());

            ViewBag.InfoModal = "true";
            ViewBag.ModalText = "کتاب ها برای شما ثبت شدند ، به تعداد روز استفاده برای هر کتاب توجه فرمایید";
            return View("ShoppingCart", model);
        }

        [Route("/DeleteShoppingCart")]
        public async Task<IActionResult> DeleteShoppingCart()
        {
            await _shoppingCartRepository.DeleteByUserId(User.GetUserId());
            var model = new DashboardDto()
            {
                ShoppingCartBooks = await _shoppingCartRepository.GetShoppingCartByUserId(User.GetUserId()),
            };
            await GetMenuData();
            ViewBag.InfoModal = "true";
            ViewBag.ModalText = "لیست امانات شما به روز رسانی شد";
            return View("ShoppingCart", model);
        }


        [Route("/DeleteItemShoppingCart/{bookId}")]
        public async Task<IActionResult> DeleteItemShoppingCart(int bookId)
        {
            await _shoppingCartRepository.Delete(bookId, User.GetUserId());
            var model = new DashboardDto()
            {
                ShoppingCartBooks = await _shoppingCartRepository.GetShoppingCartByUserId(User.GetUserId()),
            };
            await GetMenuData();
            ViewBag.InfoModal = "true";
            ViewBag.ModalText = "لیست امانات شما به روز رسانی شد";
            return View("ShoppingCart", model);
        }
        #endregion

        #region History

        [Route("History")]
        public async Task<IActionResult> History()
        {
            var model = new DashboardDto()
            {
                OldBooks = await _userBookRepository.GetItemByUserId(User.GetUserId()),
            };
            await GetMenuData();
            ViewBag.Title = "داشبرد";
            return View(model);
        }

        #endregion

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
