using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UniversityProject.Core.DTOs;
using UniversityProject.Core.Repositories;
using UniversityProject.Data.Context;
using UniversityProject.Domain.Entities;

namespace UniversityProject.Core.Services
{
    public class CategoryService : ICategoryRepository
    {
        private readonly UniversityProjectContext _context;

        public CategoryService(UniversityProjectContext context)
        {
            _context = context;
        }
        public async Task Delete(int id)
        {
            var category = await _context.Categories.FindAsync(id);
            if (category != null)
            {
                _context.Categories.Remove(category);
            }

            await _context.SaveChangesAsync();
        }

        public async Task<List<Category>> GetAll()
        {
            return await _context.Categories.ToListAsync();
        }

        public async Task<Category> GetItem(int id)
        {
            return await _context.Categories.FindAsync(id);
        }

        public async Task<CategoryListDto> GetItemsByPaging(CategorySearchDto dto)
        {
            IQueryable<Category> categories = _context.Categories;
            if (string.IsNullOrWhiteSpace(dto.Title)==false)
            {
                categories = categories.Where(x => x.Title.Contains(dto.Title));
            }

            var count = await categories.CountAsync();

            categories = categories.OrderBy(u => u.Id)
                .Skip((dto.CurrentPage - 1) * dto.ItemPerPage)
                .Take(dto.ItemPerPage);

            return new CategoryListDto()
            {
                Categories = await categories.ToListAsync(),
                CurrentPage = dto.CurrentPage,
                TotalItems = count,
                ItemPerPage = dto.ItemPerPage,
                UrlParam = dto.UrlParam
            };
        }

        public async Task<string> Insert(Category category)
        {
            if (string.IsNullOrWhiteSpace(category.Title))
            {
                return "عنوان را وارد نمایید";
            }

            await _context.AddAsync(category);
            await _context.SaveChangesAsync();
            return null;
        }

        public async Task<string> Update(Category category)
        {
            if (string.IsNullOrWhiteSpace(category.Title))
            {
                return "عنوان را وارد نمایید";
            }

            _context.Update(category);
            await _context.SaveChangesAsync();
            return null;
        }
    }
}
