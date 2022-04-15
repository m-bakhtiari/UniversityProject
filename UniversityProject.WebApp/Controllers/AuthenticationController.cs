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

        [Route("/Login")]
        public async Task<IActionResult> Login()
        {
            ViewData["Category"] = await _categoryRepository.GetAll();
            if (User.Identity.IsAuthenticated)
            {
                ViewBag.WishListCount = await _favoriteBookRepository.CountByUserId(User.GetUserId());
                ViewBag.CartCount = await _shoppingCartRepository.CountByUserId(User.GetUserId());
            }

            return View(new LoginDto());
        }

        [HttpPost]
        [Route("/Login")]
        public async Task<IActionResult> Login(LoginDto login, string returnUrl = "/")
        {
            login.Password = PasswordHelper.EncodePasswordMd5(login.Password);
            var user = await _userRepository.LoginUser(new User() { Phone = login.Username, Password = login.Password });
            if (user != null)
            {
                var claims = new List<Claim>()
                    {
                        new Claim(ClaimTypes.NameIdentifier,user.Id.ToString()),
                        new Claim(ClaimTypes.Name,user.Name)
                    };
                var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var principal = new ClaimsPrincipal(identity);

                var properties = new AuthenticationProperties
                {
                    IsPersistent = login.RememberMe
                };
                await HttpContext.SignInAsync(principal, properties);

                ViewBag.IsSuccess = true;
                if (returnUrl != "/")
                {
                    return Redirect(returnUrl);
                }
                return Redirect("Dashboard");
            }
            return View("Login");
        }


        [HttpPost("/Register")]
        public async Task<string> Register(LoginDto loginDto)
        {
            if (loginDto.Password != loginDto.RePassword)
            {
                return null;
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
                return res;
            }
            return null;
        }
    }
}
