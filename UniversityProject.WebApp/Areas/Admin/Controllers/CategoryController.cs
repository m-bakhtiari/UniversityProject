using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
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

        public async Task<IActionResult> Index()
        {
            var model = await _categoryRepository.GetAll();
            return View(model);
        }

        [HttpGet("/Admin/Category/Create/{id?}")]
        public async Task<IActionResult> Create(int? id)
        {
            ViewBag.ShowModal = "false";
            var groups = await _categoryRepository.GetMainGroups();
            var gradeType = new List<SelectListItem>();
            if (id.HasValue)
            {
                var category = await _categoryRepository.GetItem(id.Value);
                gradeType.Add(new SelectListItem()
                {
                    Text = category.Title,
                    Value = category.Id.ToString(),
                    Disabled = true
                });
            }
            else
            {
                foreach (var item in groups)
                {
                    gradeType.Add(new SelectListItem()
                    {
                        Text = item.Title,
                        Value = item.Id.ToString(),
                    });
                }
                ViewBag.GradeTitles = gradeType;
            }

            var model = new Category() { ParentId = id };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Create(Category category)
        {
            var res = await _categoryRepository.Insert(category);
            if (string.IsNullOrWhiteSpace(res) == false)
            {
                ViewBag.ShowModal = "true";
                return View(category);
            }

            return Redirect("/Admin/Category");
        }
    }
}
