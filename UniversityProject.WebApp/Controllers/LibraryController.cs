﻿using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using UniversityProject.Core.DTOs;
using UniversityProject.Core.Repositories;

namespace UniversityProject.WebApp.Controllers
{
    public class LibraryController : Controller
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IBookRepository _bookRepository;

        public LibraryController(ICategoryRepository categoryRepository, IBookRepository bookRepository)
        {
            _categoryRepository = categoryRepository;
            _bookRepository = bookRepository;
        }

        [HttpGet]
        public async Task<IActionResult> Index(LibraryDto libraryDto)
        {
            ViewData["Category"] = await _categoryRepository.GetAll();
            var model = await _bookRepository.GetLibraryData(libraryDto);
            return View(model);
        }

    }
}
