using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using UniversityProject.Core.Repositories;
using UniversityProject.Core.Utils;

namespace UniversityProject.WebApp.Areas.Admin.Controllers
{
    [Area("Admin")]
    [PermissionChecker]
    public class MessageController : Controller
    {
        private readonly IMessageRepository _messageRepository;

        public MessageController(IMessageRepository messageRepository)
        {
            _messageRepository = messageRepository;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _messageRepository.GetAll());
        }

        public async Task<IActionResult> UpdateStatus(int id)
        {
            await _messageRepository.ToggleStatus(id);
            return Redirect("/Admin/Message");
        }

        public async Task<IActionResult> Details(int id)
        {
            var message = await _messageRepository.GetItem(id);
            return View(message);
        }
    }
}
