using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using UniversityProject.Core.DTOs;
using UniversityProject.Core.Repositories;
using UniversityProject.Core.Utils;
using UniversityProject.Data.Consts;
using UniversityProject.Data.Entities;

namespace UniversityProject.WebApp.Controllers
{
    public class AuthenticationController : Controller
    {
        private readonly IUserRepository _userRepository;
        private readonly IFavoriteBookRepository _favoriteBookRepository;
        private readonly IShoppingCartRepository _shoppingCartRepository;
        private readonly ICategoryRepository _categoryRepository;

        public AuthenticationController(IUserRepository userRepository, IFavoriteBookRepository favoriteBookRepository, IShoppingCartRepository shoppingCartRepository, ICategoryRepository categoryRepository)
        {
            _userRepository = userRepository;
            _favoriteBookRepository = favoriteBookRepository;
            _shoppingCartRepository = shoppingCartRepository;
            _categoryRepository = categoryRepository;
        }

        [HttpGet("/Login")]
        [HttpGet("/Login/{returnUrl}")]
        public async Task<IActionResult> Login(string returnUrl)
        {
            await GetMenuData();
            ViewBag.Title = "ورود / ثبت نام";
            return View(new LoginDto() { ReturnUrl = returnUrl });
        }

        [HttpPost("/Login")]
        public async Task<IActionResult> Login(LoginDto login)
        {
            if (string.IsNullOrWhiteSpace(login.Password) == false)
            {
                login.Password = PasswordHelper.EncodePasswordMd5(login.Password);
            }
            var user = await _userRepository.LoginUser(new User() { Phone = login.Username, Password = login.Password });
            if (user != null)
            {
                var claims = new List<Claim>()
                {
                    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                    new Claim(ClaimTypes.Name, user.Name),
                    new Claim(ClaimTypes.Role, user.RoleId.ToString())
                };
                var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var principal = new ClaimsPrincipal(identity);
                var properties = new AuthenticationProperties
                {
                    IsPersistent = login.RememberMe
                };
                await HttpContext.SignInAsync(principal, properties);
                if (!string.IsNullOrWhiteSpace(login.ReturnUrl))
                {
                    return Redirect(login.ReturnUrl);
                }
                return Redirect("/Dashboard");
            }
            await GetMenuData();
            ViewBag.ErrorModal = "true";
            ViewBag.ErrorMessage = "کاربری با این اطلاعات یافت نشد";
            ViewBag.Title = "ورود / ثبت نام";

            return View("Login", new LoginDto() { ReturnUrl = login.ReturnUrl });
        }


        [HttpPost("/Register")]
        public async Task<IActionResult> Register(LoginDto loginDto)
        {
            await GetMenuData();
            ViewBag.Title = "لاگین";
            if (loginDto.Password != loginDto.RePassword)
            {
                ViewBag.ErrorModal = "true";
                ViewBag.ErrorMessage = "رمز عبور با تکرار آن یکسان نمی باشد";
                ViewBag.Title = "ورود / ثبت نام";

                return View("Login", loginDto);
            }
            var user = new User()
            {
                Password = loginDto.Password,
                Phone = loginDto.Username,
                RoleId = Const.UserRoleId,
                Name = loginDto.Name
            };
            var res = await _userRepository.Insert(user);
            if (string.IsNullOrWhiteSpace(res) == false)
            {
                ViewBag.ErrorModal = "true";
                ViewBag.ErrorMessage = res;
                ViewBag.Title = "ورود / ثبت نام";

                return View("Login", loginDto);
            }
            ViewBag.InfoModal = "true";
            ViewBag.InfoMessage = "ثبت نام با موفقیت انجام شد . اکنون لاگین نمایید";
            ViewBag.Title = "ورود / ثبت نام";

            return View("Login", new LoginDto());

        }

        [Route("ForgotPassword")]
        public async Task<IActionResult> ForgotPassword()
        {
            await GetMenuData();
            ViewBag.Title = "فراموشی رمز عبور";
            return View("ForgotPassword", new LoginDto());
        }

        [HttpPost("AddUserCode")]
        public async Task<IActionResult> AddUserCode(LoginDto loginDto)
        {
            await GetMenuData();
            var user = await _userRepository.GetItemByPhoneNumber(loginDto.Username);
            if (user == null)
            {
                ViewBag.Title = "فراموشی رمز عبور";
                ViewBag.ErrorModal = "true";
                ViewBag.ErrorMessage = "کاربری یا این شماره یافت نشد";
                return View("ForgotPassword", new LoginDto());
            }
            ViewBag.Title = "وارد کردن کد";
            ViewBag.InfoModal = "true";
            ViewBag.InfoMessage = "کد به شماره تماس وارد شده ارسال شد ،کد را وارد نمایید";
            return View("AddUserCode", new LoginDto() { Username = user.Phone, Code = user.Id });
        }

        [HttpPost("ResetPassword")]
        public async Task<IActionResult> ResetPassword(LoginDto loginDto)
        {
            await GetMenuData();
            if (loginDto.Id != null)
            {
                var userData = await _userRepository.GetUserByCode(loginDto.Username, loginDto.Id.Value);
                if (userData)
                {
                    await _userRepository.ResetPassword(loginDto.Username, loginDto.Id.Value);
                    ViewBag.InfoModal = "true";
                    ViewBag.InfoMessage = "رمز عبور شما به 123456 تغییر یافت از طریق داشبرد می توانید رمز عبور خود را تغییر دهید";
                    ViewBag.Title = "ورود / ثبت نام";
                    return View("Login", new LoginDto());
                }
            }
            ViewBag.Title = "وارد کردن کد";
            ViewBag.ErrorModal = "true";
            ViewBag.ErrorMessage = "کد وارد شده معتبر نمی باشد";
            var user = await _userRepository.GetItemByPhoneNumber(loginDto.Username);
            loginDto.Code = user.Id;
            return View("AddUserCode", loginDto);
        }

        [Route("/Logout")]
        public async Task<IActionResult> LogOut()
        {
            await HttpContext.SignOutAsync();
            return Redirect("/");
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
