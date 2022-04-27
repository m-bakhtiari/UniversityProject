using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
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
            return View(new LoginDto() { ReturnUrl = returnUrl });
        }

        [HttpPost("/Login")]
        public async Task<IActionResult> Login(LoginDto login)
        {
            login.Password = PasswordHelper.EncodePasswordMd5(login.Password);
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
            return View("Login", new LoginDto() { ReturnUrl = login.ReturnUrl });
        }


        [HttpPost("/Register")]
        public async Task<IActionResult> Register(LoginDto loginDto)
        {
            if (loginDto.Password != loginDto.RePassword)
            {
                await GetMenuData();
                ViewBag.ErrorModal = "true";
                ViewBag.ErrorMessage = "رمز عبور با تکرار آن یکسان نمی باشد";
                return View("Login");
            }
            var user = new User()
            {
                Password = loginDto.Password,
                Phone = loginDto.Username,
                RoleId = Const.UserRoleId,
            };
            var res = await _userRepository.Insert(user);
            if (string.IsNullOrWhiteSpace(res))
            {
                ViewBag.ErrorModal = "true";
                ViewBag.ErrorMessage = res;
                return View("Login");
            }
            return null;
        }

        [Route("ForgotPassword")]
        public async Task<IActionResult> ForgotPassword()
        {
            await GetMenuData();
            return View("ForgotPassword");
        }

        [HttpPost("AddUserCode")]
        public async Task<IActionResult> AddUserCode(LoginDto loginDto)
        {
            await GetMenuData();
            ViewBag.ErrorModal = "true";
            ViewBag.ErrorMessage = "کد ارسالی به شماره تماس را وارد نمایید";
            var user = await _userRepository.GetItemByPhoneNumber(loginDto.Username);
            if (user == null)
            {
                return View("ForgotPassword");
            }
            return View("AddUserCode", new LoginDto() { Username = user.Phone, Code = user.Id });
        }

        [HttpPost("ResetPassword")]
        public async Task<IActionResult> ResetPassword(LoginDto loginDto)
        {
            if (loginDto.Id != null)
            {
                var userData = await _userRepository.GetUserByCode(loginDto.Username, loginDto.Id.Value);
                if (userData)
                {
                    await _userRepository.ResetPassword(loginDto.Username, loginDto.Id.Value);
                }
            }
            await GetMenuData();
            ViewBag.ErrorModal = "true";
            ViewBag.ErrorMessage = "رمز عبور شما به 1234 تغییر یافت از طریق داشبرد می توانید رمز عبور خود را تغییر دهید";
            return View("Login");
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
