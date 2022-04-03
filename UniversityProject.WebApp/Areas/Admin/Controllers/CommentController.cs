using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UniversityProject.Core.Repositories;
using UniversityProject.Data.Entities;

namespace UniversityProject.WebApp.Areas.Admin.Controllers
{
    [Area("Admin")]
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

        public IActionResult Create(int parentId)
        {
            ViewBag.ReturnUrl = "/Admin/Comment/Create";
            ViewBag.ShowModal = "false";
            return View(new Comment() { ParentId = parentId });
        }

        [HttpPost]
        public async Task<IActionResult> Create(Comment comment)
        {
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
            if (model.ParentId != null) ViewBag.UserComment = await _commentRepository.GetItem(model.ParentId.Value);
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
            await _commentRepository.Delete(id);
            return Redirect("/Admin/Comment");
        }
    }
}
