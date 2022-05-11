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
    public class TeamController : Controller
    {
        private readonly ITeamRepository _teamRepository;

        public TeamController(ITeamRepository teamRepository)
        {
            _teamRepository = teamRepository;
        }
        public async Task<IActionResult> Index()
        {
            var model = await _teamRepository.GetAll();
            return View(model);
        }
        public async Task<IActionResult> Create()
        {
            ViewBag.ReturnUrl = "/Admin/Team/Create";
            ViewBag.ShowModal = "false";
            var model = new TeamDto();
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Create(TeamDto teamDto)
        {
            ViewBag.ReturnUrl = "/Admin/Team/Create";
            ViewBag.ShowModal = "false";

            var team = new Team()
            {
                JobTitle = teamDto.JobTitle,
                Facebook = teamDto.Facebook,
                Instagram = teamDto.Instagram,
                Name = teamDto.Name,
                Twitter = teamDto.Twitter
            };
            await _teamRepository.Insert(team, teamDto.Image);
            return Redirect("/Admin/Team");
        }

        public async Task<IActionResult> Delete(int id)
        {
            await _teamRepository.Delete(id);
            return Redirect("/Admin/Team");
        }

        public async Task<IActionResult> Update(int id)
        {
            ViewBag.ReturnUrl = "/Admin/Team/Update";
            ViewBag.ShowModal = "false";
            var team = await _teamRepository.GetItem(id);
            var model = new TeamDto()
            {
                JobTitle = team.JobTitle,
                Name = team.Name,
                OldImage = team.Image,
                Id = team.Id,
                Facebook = team.Facebook,
                Instagram = team.Instagram,
                Twitter = team.Twitter
            };
            return View("Create", model);
        }

        [HttpPost]
        public async Task<IActionResult> Update(TeamDto teamDto)
        {
            ViewBag.ReturnUrl = "/Admin/Team/Update";
            ViewBag.ShowModal = "false";
            var team = new Team()
            {
                JobTitle = teamDto.JobTitle,
                Name = teamDto.Name,
                Id = teamDto.Id,
                Image = teamDto.OldImage,
                Facebook = teamDto.Facebook,
                Instagram = teamDto.Instagram,
                Twitter = teamDto.Twitter
            };
            await _teamRepository.Update(team, teamDto.Image);
            return Redirect("/Admin/Team");
        }
    }
}
