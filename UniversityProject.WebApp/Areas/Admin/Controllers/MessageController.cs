using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using UniversityProject.Core.Repositories;

namespace UniversityProject.WebApp.Areas.Admin.Controllers
{
    [Area("Admin")]
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
    }
}
