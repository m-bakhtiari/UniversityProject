using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using UniversityProject.Core.Repositories;
using UniversityProject.Data.Entities;

namespace UniversityProject.WebApp.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CategoryController : Controller
    {
        private readonly ICategoryRepository _categoryRepository;

        public CategoryController(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var model = await _categoryRepository.GetAll();
            return View(model);
        }

        [HttpGet("/Admin/Category/Create/{id?}")]
        public IActionResult Create(int? id)
        {
            ViewBag.ReturnUrl = "/Admin/Category/Create";
            var model = new Category() { ParentId = id };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Create(Category category)
        {
            ViewBag.ReturnUrl = "/Admin/Category/Create";
            var res = await _categoryRepository.Insert(category);
            if (string.IsNullOrWhiteSpace(res) == false)
            {
                ViewBag.ShowModal = "true";
                return View(category);
            }
            return Redirect("/Admin/Category");
        }

        [HttpGet("/Admin/Category/Update/{id:int}")]
        public async Task<IActionResult> Update(int id)
        {
            ViewBag.ReturnUrl = "/Admin/Category/Update";
            var model = await _categoryRepository.GetItem(id);
            return View("Create", model);
        }

        [HttpPost]
        public async Task<IActionResult> Update(Category category)
        {
            ViewBag.ReturnUrl = "/Admin/Category/Update";
            var res = await _categoryRepository.Update(category);
            if (string.IsNullOrWhiteSpace(res) == false)
            {
                ViewBag.ShowModal = "true";
                return View("Create", category);
            }
            return Redirect("/Admin/Category");
        }

        public async Task<IActionResult> Delete(int id)
        {
            await _categoryRepository.Delete(id);
            return Redirect("/Admin/Category");
        }
    }
}
