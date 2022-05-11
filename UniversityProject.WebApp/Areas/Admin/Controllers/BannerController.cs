using System;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UniversityProject.Core.DTOs;
using UniversityProject.Core.Repositories;
using UniversityProject.Core.Utils;
using UniversityProject.Data.Entities;

namespace UniversityProject.WebApp.Areas.Admin.Controllers
{
    [Area("Admin")]
    [PermissionChecker]
    public class BannerController : Controller
    {
        private readonly IBannerRepository _bannerRepository;
        private readonly IBookRepository _bookRepository;
        private readonly ICategoryRepository _categoryRepository;

        public BannerController(IBannerRepository bannerRepository, IBookRepository bookRepository, ICategoryRepository categoryRepository)
        {
            _bannerRepository = bannerRepository;
            _bookRepository = bookRepository;
            _categoryRepository = categoryRepository;
        }

        public async Task<IActionResult> Index()
        {
            var model = await _bannerRepository.GetAll();
            foreach (var item in model.Where(item => string.IsNullOrWhiteSpace(item.LinkUrl) == false))
            {
                if (item.LinkUrl.Contains("Book"))
                {
                    var bookId = item.LinkUrl.Split("/").LastOrDefault();
                    var book = await _bookRepository.GetItem(Convert.ToInt32(bookId));
                    item.LinkUrl = book.Title;
                }
                else if (item.LinkUrl.Contains("category"))
                {
                    var bannerId = item.LinkUrl.Split("=").LastOrDefault();
                    var banner = await _categoryRepository.GetItem(Convert.ToInt32(bannerId));
                    item.LinkUrl = banner.Title;
                }
            }
            return View(model);
        }

        public async Task<IActionResult> Create()
        {
            ViewBag.ReturnUrl = "/Admin/Banner/Create";
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
            var model = new BannerDto()
            {
                BookTitles = linkList
            };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Create(BannerDto bannerDto)
        {
            ViewBag.ReturnUrl = "/Admin/Banner/Create";
            ViewBag.ShowModal = "false";
            var banner = new Banner()
            {
                Title = bannerDto.Title,
                Position = bannerDto.Position,
                LinkUrl = bannerDto.LinkUrl
            };
            var addBanner = await _bannerRepository.Insert(banner, bannerDto.Image);
            if (string.IsNullOrWhiteSpace(addBanner) == false)
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
                bannerDto.BookTitles = linkList;
                ViewBag.ShowModal = "true";
                return View(bannerDto);
            }
            return Redirect("/Admin/Banner");
        }

        public async Task<IActionResult> Update(int id)
        {
            ViewBag.ReturnUrl = "/Admin/Banner/Update";
            ViewBag.ShowModal = "false";
            var banner = await _bannerRepository.GetItem(id);
            var linkList = new List<BookTitle>();
            linkList.AddRange(await _bookRepository.GetAllTitles());
            var category = await _categoryRepository.GetAll();
            linkList.AddRange(category.Select(x => new BookTitle()
            {
                Id = x.Id,
                Title = x.Title,
                LinkType = "BookCategory"
            }));
            var model = new BannerDto()
            {
                BookTitles = linkList,
                Title = banner.Title,
                Id = banner.Id,
                Position = banner.Position,
                ImageName = banner.Image,
                LinkUrl = banner.LinkUrl
            };
            return View("Create", model);
        }

        [HttpPost]
        public async Task<IActionResult> Update(BannerDto bannerDto)
        {
            ViewBag.ReturnUrl = "/Admin/Banner/Update";
            ViewBag.ShowModal = "false";
            var banner = new Banner()
            {
                Title = bannerDto.Title,
                Position = bannerDto.Position,
                Id = bannerDto.Id,
                LinkUrl = bannerDto.LinkUrl,
                Image = bannerDto.ImageName
            };
            var addBanner = await _bannerRepository.Update(banner, bannerDto.Image);
            if (string.IsNullOrWhiteSpace(addBanner) == false)
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
                bannerDto.BookTitles = linkList;
                ViewBag.ShowModal = "true";
                return View("Create", bannerDto);
            }
            return Redirect("/Admin/Banner");
        }

        public async Task<IActionResult> Delete(int id)
        {
            await _bannerRepository.Delete(id);
            return Redirect("/Admin/Banner");
        }
    }
}
