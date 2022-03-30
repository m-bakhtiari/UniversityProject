using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using UniversityProject.Core.Repositories;
using UniversityProject.Data.Entities;

namespace UniversityProject.WebApp.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class RoleController : Controller
    {
        private readonly IRoleRepository _roleRepository;
        public RoleController(IRoleRepository roleRepository)
        {
            _roleRepository = roleRepository;
        }

        public async Task<IActionResult> Index()
        {
            var model = await _roleRepository.GetAll();
            return View(model);
        }

        public IActionResult Create()
        {
            ViewBag.ReturnUrl = "/Admin/Role/Create";
            ViewBag.ShowModal = "false";
            return View(new Role());
        }

        [HttpPost]
        public async Task<IActionResult> Create(Role role)
        {
            var addRole = await _roleRepository.Insert(role);
            if (string.IsNullOrWhiteSpace(addRole) == false)
            {
                ViewBag.ReturnUrl = "/Admin/Role/Create";
                ViewBag.ShowModal = "true";
                return View("Create", role);
            }
            return Redirect("/Admin/Role");
        }

        public async Task<IActionResult> Update(int id)
        {
            var model = await _roleRepository.GetItem(id);
            ViewBag.ReturnUrl = "/Admin/Role/Update";
            ViewBag.ShowModal = "false";
            return View("Create", model);
        }

        [HttpPost]
        public async Task<IActionResult> Update(Role role)
        {
            var addRole = await _roleRepository.Update(role);
            if (string.IsNullOrWhiteSpace(addRole) == false)
            {
                ViewBag.ReturnUrl = "/Admin/Role/Update";
                ViewBag.ShowModal = "true";
                return View("Create", role);
            }
            return Redirect("/Admin/Role");
        }

        public async Task<IActionResult> Delete(int id)
        {
            await _roleRepository.Delete(id);
            return Redirect("/Admin/Role");
        }
    }
}
