using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using UniversityProject.Core.DTOs;
using UniversityProject.Core.Repositories;
using UniversityProject.Core.Utils;

namespace UniversityProject.WebApp.Areas.Admin.Controllers
{
    [Area("Admin")]
    [PermissionChecker]
    public class HomeController : Controller
    {
        private readonly IUserRepository _userRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly IBookRepository _bookRepository;
        private readonly ITeamRepository _teamRepository;
        private readonly ICommentRepository _commentRepository;
        private readonly IBannerRepository _bannerRepository;
        private readonly ISliderRepository _sliderRepository;
        private readonly IUserBookRepository _userBookRepository;
        private readonly IMessageRepository _messageRepository;

        public HomeController(IUserRepository userRepository, ICategoryRepository categoryRepository, IBookRepository bookRepository, ITeamRepository teamRepository, ICommentRepository commentRepository, IBannerRepository bannerRepository, ISliderRepository sliderRepository, IUserBookRepository userBookRepository, IMessageRepository messageRepository)
        {
            _userRepository = userRepository;
            _categoryRepository = categoryRepository;
            _bookRepository = bookRepository;
            _teamRepository = teamRepository;
            _commentRepository = commentRepository;
            _bannerRepository = bannerRepository;
            _sliderRepository = sliderRepository;
            _userBookRepository = userBookRepository;
            _messageRepository = messageRepository;
        }

        public async Task<IActionResult> Index()
        {
            var model = new AdminPanelDto()
            {
                BookCount = await _bookRepository.BookCount(),
                CategoryCount = await _categoryRepository.CategoryCount(),
                CommentCount = await _commentRepository.CommentCount(),
                UserCount = await _userRepository.UserCount(),
                Penalty = await _userRepository.PenaltySum(),
                BannerCount = await _bannerRepository.BannerCount(),
                MessageCount = await _messageRepository.MessageCount(),
                SliderCount = await _sliderRepository.SliderCount(),
                UnreadMessageCount = await _messageRepository.UnreadMessageCount(),
                UserBookCount = await _userBookRepository.UserBookCount(),
                UserBookNotReturn = await _userBookRepository.UserBookNotReturn(),
                TeamCount = await _teamRepository.TeamCount()
            };
            return View(model);
        }
    }
}
