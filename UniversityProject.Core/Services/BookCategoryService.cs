using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using UniversityProject.Core.Repositories;
using UniversityProject.Data.Context;
using UniversityProject.Data.Entities;

namespace UniversityProject.Core.Services
{
    public class BookCategoryService : IBookCategoryRepository
    {
        private readonly UniversityProjectContext _context;

        public BookCategoryService(UniversityProjectContext context)
        {
            _context = context;
        }
        public async Task Delete(int bookId, int categoryId)
        {
            var model = await _context.BookCategories.FirstOrDefaultAsync(x =>
                x.BookId == bookId && x.CategoryId == categoryId);
            _context.BookCategories.Remove(model);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteByBookId(int bookId)
        {
            var book = await _context.BookCategories.Where(x => x.BookId == bookId).ToListAsync();
            _context.BookCategories.RemoveRange(book);
        }

        public async Task<List<BookCategory>> GetAll()
        {
            return await _context.BookCategories.ToListAsync();
        }

        public async Task<BookCategory> GetItem(int bookId, int categoryId)
        {
            return await _context.BookCategories.FirstOrDefaultAsync(x =>
                x.BookId == bookId && x.CategoryId == categoryId);
        }

        public async Task<List<BookCategory>> GetItemByBookId(int bookId)
        {
            return await _context.BookCategories.Where(x => x.BookId == bookId).ToListAsync();
        }

        public async Task<List<BookCategory>> GetItemByCategoryId(int categoryId)
        {
            return await _context.BookCategories.Where(x => x.CategoryId == categoryId).ToListAsync();
        }

        public async Task<string> Insert(BookCategory bookCategory)
        {
            await _context.BookCategories.AddAsync(bookCategory);
            await _context.SaveChangesAsync();
            return null;
        }
    }
}
