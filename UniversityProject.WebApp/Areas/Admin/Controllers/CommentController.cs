using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using UniversityProject.Core.Repositories;
using UniversityProject.Core.Utils;
using UniversityProject.Data.Entities;

namespace UniversityProject.WebApp.Areas.Admin.Controllers
{
    [Area("Admin")]
    [PermissionChecker]
    public class CommentController : Controller
    {
        private readonly ICommentRepository _commentRepository;
        public CommentController(ICommentRepository commentRepository)
        {
            _commentRepository = commentRepository;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _commentRepository.GetAll());
        }

        public async Task<IActionResult> Create(int parentId)
        {
            ViewBag.ReturnUrl = "/Admin/Comment/Create";
            ViewBag.ShowModal = "false";
            var comment = await _commentRepository.GetItem(parentId);
            ViewBag.UserComment = comment.Text;
            return View(new Comment() { ParentId = parentId, BookId = comment.BookId });
        }

        [HttpPost]
        public async Task<IActionResult> Create(Comment comment)
        {
            comment.UserId = User.GetUserId();
            var addComment = await _commentRepository.Insert(comment);
            if (string.IsNullOrWhiteSpace(addComment) == false)
            {
                ViewBag.ReturnUrl = "/Admin/Comment/Create";
                ViewBag.ShowModal = "true";
                return View("Create", comment);
            }
            return Redirect("/Admin/Comment");
        }

        public async Task<IActionResult> Update(int id)
        {
            var model = await _commentRepository.GetItem(id);
            ViewBag.ReturnUrl = "/Admin/Comment/Update";
            ViewBag.ShowModal = "false";
            if (model.ParentId == null) return View("Create", model);
            var comment= await _commentRepository.GetItem(model.ParentId.Value);
            if (model.ParentId != null) ViewBag.UserComment = comment.Text;

            return View("Create", model);
        }

        [HttpPost]
        public async Task<IActionResult> Update(Comment comment)
        {
            var addComment = await _commentRepository.Update(comment);
            if (string.IsNullOrWhiteSpace(addComment) == false)
            {
                ViewBag.ReturnUrl = "/Admin/Comment/Update";
                ViewBag.ShowModal = "true";
                return View("Create", comment);
            }
            return Redirect("/Admin/Comment");
        }

        public async Task<IActionResult> Delete(int id)
        {
            if (await _commentRepository.CommentHasAnswer(id))
            {
                ViewBag.ShowModal = "true";
                ViewBag.ModalMessage = "ابتدا جواب کامنت را حذف نمایید";
                return View("Index",await _commentRepository.GetAll());
            }
            await _commentRepository.Delete(id);
            return Redirect("/Admin/Comment");
        }
    }
}
