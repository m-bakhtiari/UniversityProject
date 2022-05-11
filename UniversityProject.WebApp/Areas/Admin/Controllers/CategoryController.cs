using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using UniversityProject.Core.Repositories;
using UniversityProject.Core.Utils;
using UniversityProject.Data.Entities;

namespace UniversityProject.WebApp.Areas.Admin.Controllers
{
    [Area("Admin")]
    [PermissionChecker]
    public class CategoryController : Controller
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IBookCategoryRepository _bookCategoryRepository;

        public CategoryController(ICategoryRepository categoryRepository, IBookCategoryRepository bookCategoryRepository)
        {
            _categoryRepository = categoryRepository;
            _bookCategoryRepository = bookCategoryRepository;
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
            if (await _categoryRepository.IsMainGroup(id))
            {
                ViewBag.ShowModal="true";
                ViewBag.ModalMessage = "ابتدا زیر گروه ها را حذف نمایید";
                return View("Index", await _categoryRepository.GetAll());
            }
            else if (await _bookCategoryRepository.IsExistBookByCategoryId(id))
            {
                ViewBag.ShowModal = "true";
                ViewBag.ModalMessage = "ابتدا آیتم های مرتبط با این زیر گروه را حذف نمایید مانند بنرها ، کتاب ها و...";
                return View("Index", await _categoryRepository.GetAll());
            }
            await _categoryRepository.Delete(id);
            return Redirect("/Admin/Category");
        }
    }
}
