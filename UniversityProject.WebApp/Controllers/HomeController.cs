using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using UniversityProject.Core.DTOs;
using UniversityProject.Core.Repositories;

namespace UniversityProject.WebApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IBannerRepository _bannerRepository;
        private readonly IBookRepository _bookRepository;
        private readonly ISliderRepository _sliderRepository;

        public HomeController(ICategoryRepository categoryRepository, IBannerRepository bannerRepository, IBookRepository bookRepository, ISliderRepository sliderRepository)
        {
            _categoryRepository = categoryRepository;
            _bannerRepository = bannerRepository;
            _bookRepository = bookRepository;
            _sliderRepository = sliderRepository;
        }

        public async Task<IActionResult> Index()
        {
            ViewData["Category"] = await _categoryRepository.GetAll();
            var model = new MainPageDto()
            {
                Banners = await _bannerRepository.GetAll(),
                Sliders = await _sliderRepository.GetAll(),
                FavoriteBooks = await _bookRepository.GetLatestBook(),
                LatestBooks = await _bookRepository.GetPopularBooks(),
            };
            return View(model);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View();
        }
    }
}
