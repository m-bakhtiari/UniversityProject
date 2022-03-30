using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using UniversityProject.Core.Repositories;
using UniversityProject.Data.Entities;

namespace UniversityProject.WebApp.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class SubscriptionTypeController : Controller
    {
        private readonly ISubscriptionTypeRepository _subscriptionTypeRepository;

        public SubscriptionTypeController(ISubscriptionTypeRepository subscriptionTypeRepository)
        {
            _subscriptionTypeRepository = subscriptionTypeRepository;
        }

        public async Task<IActionResult> Index()
        {
            var model = await _subscriptionTypeRepository.GetAll();
            return View(model);
        }

        public IActionResult Create()
        {
            ViewBag.ReturnUrl = "/Admin/SubscriptionType/Create";
            ViewBag.ShowModal = "false";
            return View(new SubscriptionType());
        }

        [HttpPost]
        public async Task<IActionResult> Create(SubscriptionType subscriptionType)
        {
            var addSubscriptionType = await _subscriptionTypeRepository.Insert(subscriptionType);
            if (string.IsNullOrWhiteSpace(addSubscriptionType) == false)
            {
                ViewBag.ReturnUrl = "/Admin/SubscriptionType/Create";
                ViewBag.ShowModal = "true";
                return View("Create", subscriptionType);
            }
            return Redirect("/Admin/SubscriptionType");
        }

        public async Task<IActionResult> Update(int id)
        {
            var model = await _subscriptionTypeRepository.GetItem(id);
            ViewBag.ReturnUrl = "/Admin/SubscriptionType/Update";
            ViewBag.ShowModal = "false";
            return View("Create", model);
        }

        [HttpPost]
        public async Task<IActionResult> Update(SubscriptionType subscriptionType)
        {
            var addSubscriptionType = await _subscriptionTypeRepository.Update(subscriptionType);
            if (string.IsNullOrWhiteSpace(addSubscriptionType) == false)
            {
                ViewBag.ReturnUrl = "/Admin/SubscriptionType/Update";
                ViewBag.ShowModal = "true";
                return View("Create", subscriptionType);
            }
            return Redirect("/Admin/SubscriptionType");
        }

        public async Task<IActionResult> Delete(int id)
        {
            await _subscriptionTypeRepository.Delete(id);
            return Redirect("/Admin/SubscriptionType");
        }
    }
}

