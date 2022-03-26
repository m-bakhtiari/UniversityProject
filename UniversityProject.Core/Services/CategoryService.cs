using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UniversityProject.Core.Repositories;
using UniversityProject.Data.Context;
using UniversityProject.Data.Entities;

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
                if (category.ParentId == null)
                {
                    var subGroup = await _context.Categories.Where(x => x.ParentId == id).ToListAsync();
                    if (subGroup.Any())
                    {
                        _context.Categories.RemoveRange(subGroup);
                    }
                }
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

        public async Task<List<Category>> GetMainGroups()
        {
            return await _context.Categories.Where(x => x.ParentId == null).ToListAsync();
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
