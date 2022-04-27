using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using UniversityProject.Core.DTOs;
using UniversityProject.Core.Repositories;
using UniversityProject.Core.Utils;
using UniversityProject.Data.Entities;

namespace UniversityProject.WebApp.Areas.Admin.Controllers
{
    [Area("Admin")]
    [PermissionChecker]
    public class UserController : Controller
    {
        private readonly IUserRepository _userRepository;
        private readonly IRoleRepository _roleRepository;

        public UserController(IUserRepository userRepository, IRoleRepository roleRepository)
        {
            _userRepository = userRepository;
            _roleRepository = roleRepository;
        }
        public async Task<IActionResult> Index()
        {
            var model = await _userRepository.GetAll();
            return View(model);
        }

        public async Task<IActionResult> Create()
        {
            ViewBag.ReturnUrl = "/Admin/User/Create";
            ViewBag.ShowModal = "false";
            var model = new UserDto()
            {
                Roles = await _roleRepository.GetAll()
            };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Create(UserDto userDto)
        {
            var user = new User()
            {
                Name = userDto.FirstName,
                Id = userDto.Id,
                Penalty = userDto.Penalty,
                Phone = userDto.Phone,
                RoleId = userDto.RoleId,
                Password = userDto.NewPassword,
            };
            var insert = await _userRepository.Insert(user);
            if (string.IsNullOrWhiteSpace(insert) == false)
            {
                ViewBag.ReturnUrl = "/Admin/User/Create";
                ViewBag.ShowModal = "true";
                return View("Create", new UserDto() { Roles = await _roleRepository.GetAll() });
            }
            return Redirect("/Admin/User");
        }

        public async Task<IActionResult> Update(int id)
        {
            var model = await _userRepository.GetItem(id);
            ViewBag.ReturnUrl = "/Admin/User/Update";
            ViewBag.ShowModal = "false";
            var modelDto = new UserDto()
            {
                FirstName = model.Name,
                Id = model.Id,
                Roles = await _roleRepository.GetAll(),
                Penalty = model.Penalty,
                Phone = model.Phone,
                RoleId = model.RoleId,
                OldPassword = model.Password,
            };
            return View("Create", modelDto);
        }

        [HttpPost]
        public async Task<IActionResult> Update(UserDto userDto)
        {
            var user = new User()
            {
                Name = userDto.FirstName,
                Id = userDto.Id,
                Penalty = userDto.Penalty,
                Phone = userDto.Phone,
                RoleId = userDto.RoleId,
                Password = string.IsNullOrWhiteSpace(userDto.NewPassword) ? userDto.OldPassword : PasswordHelper.EncodePasswordMd5(userDto.NewPassword),
            };
            var update = await _userRepository.Update(user);
            if (string.IsNullOrWhiteSpace(update) == false)
            {
                ViewBag.ReturnUrl = "/Admin/User/Update";
                ViewBag.ShowModal = "true";
                return View("Create", new UserDto() { Roles = await _roleRepository.GetAll() });
            }
            return Redirect("/Admin/User");
        }

        public async Task<IActionResult> Delete(int id)
        {
            await _userRepository.Delete(id);
            return Redirect("/Admin/User");
        }
    }
}
