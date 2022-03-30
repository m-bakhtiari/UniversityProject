using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UniversityProject.Core.DTOs;
using UniversityProject.Core.Repositories;
using UniversityProject.Data.Entities;

namespace UniversityProject.WebApp.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class SliderController : Controller
    {
        private readonly ISliderRepository _sliderRepository;
        private readonly IBookRepository _bookRepository;
        private readonly ICategoryRepository _categoryRepository;

        public SliderController(ISliderRepository sliderRepository, IBookRepository bookRepository, ICategoryRepository categoryRepository)
        {
            _sliderRepository = sliderRepository;
            _bookRepository = bookRepository;
            _categoryRepository = categoryRepository;
        }

        public async Task<IActionResult> Index()
        {
            var model = await _sliderRepository.GetAll();
            return View(model);
        }

        public async Task<IActionResult> Create()
        {
            ViewBag.ReturnUrl = "/Admin/Slider/Create";
            ViewBag.ShowModal = "false";
            var linkList = new List<BookTitle>();
            linkList.AddRange(await _bookRepository.GetAllTitles());
            var category = await _categoryRepository.GetAll();
            linkList.AddRange(category.Select(x => new BookTitle()
            {
                Id = x.Id,
                Title = x.Title,
                LinkType = "BookCategory"
            }));
            var model = new SliderDto()
            {
                BookTitles = linkList
            };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Create(SliderDto sliderDto)
        {
            ViewBag.ReturnUrl = "/Admin/Slider/Create";
            ViewBag.ShowModal = "false";
            var slider = new Slider()
            {
                Title = sliderDto.Title,
                Position = sliderDto.Position,
                LinkUrl = sliderDto.LinkUrl
            };
            var addSlider = await _sliderRepository.Insert(slider, sliderDto.Image);
            if (string.IsNullOrWhiteSpace(addSlider) == false)
            {
                var linkList = new List<BookTitle>();
                linkList.AddRange(await _bookRepository.GetAllTitles());
                var category = await _categoryRepository.GetAll();
                linkList.AddRange(category.Select(x => new BookTitle()
                {
                    Id = x.Id,
                    Title = x.Title,
                    LinkType = "BookCategory"
                }));
                sliderDto.BookTitles = linkList;
                ViewBag.ShowModal = "true";
                return View(sliderDto);
            }
            return Redirect("/Admin/Slider");
        }

        public async Task<IActionResult> Update(int id)
        {
            ViewBag.ReturnUrl = "/Admin/Slider/Update";
            ViewBag.ShowModal = "false";
            var slider = await _sliderRepository.GetItem(id);
            var linkList = new List<BookTitle>();
            linkList.AddRange(await _bookRepository.GetAllTitles());
            var category = await _categoryRepository.GetAll();
            linkList.AddRange(category.Select(x => new BookTitle()
            {
                Id = x.Id,
                Title = x.Title,
                LinkType = "BookCategory"
            }));
            var model = new SliderDto()
            {
                BookTitles = linkList,
                Title = slider.Title,
                Id = slider.Id,
                Position = slider.Position,
                ImageName = slider.Image,
                LinkUrl = slider.LinkUrl
            };
            return View("Create", model);
        }

        [HttpPost]
        public async Task<IActionResult> Update(SliderDto sliderDto)
        {
            ViewBag.ReturnUrl = "/Admin/Slider/Update";
            ViewBag.ShowModal = "false";
            var slider = new Slider()
            {
                Title = sliderDto.Title,
                Position = sliderDto.Position,
                Id = sliderDto.Id,
                LinkUrl = sliderDto.LinkUrl,
                Image = sliderDto.ImageName
            };
            var addSlider = await _sliderRepository.Update(slider, sliderDto.Image);
            if (string.IsNullOrWhiteSpace(addSlider) == false)
            {
                var linkList = new List<BookTitle>();
                linkList.AddRange(await _bookRepository.GetAllTitles());
                var category = await _categoryRepository.GetAll();
                linkList.AddRange(category.Select(x => new BookTitle()
                {
                    Id = x.Id,
                    Title = x.Title,
                    LinkType = "BookCategory"
                }));
                sliderDto.BookTitles = linkList;
                ViewBag.ShowModal = "true";
                return View("Create", sliderDto);
            }
            return Redirect("/Admin/Slider");
        }

        public async Task<IActionResult> Delete(int id)
        {
            await _sliderRepository.Delete(id);
            return Redirect("/Admin/Slider");
        }
    }
}
