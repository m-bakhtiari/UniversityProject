using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UniversityProject.Core.DTOs;
using UniversityProject.Core.Repositories;
using UniversityProject.Data.Context;
using UniversityProject.Data.Entities;

namespace UniversityProject.Core.Services
{
    public class FavoriteBookService : IFavoriteBookRepository
    {
        private readonly UniversityProjectContext _context;
        public FavoriteBookService(UniversityProjectContext context)
        {
            _context = context;
        }

        public async Task<int> CountByUserId(int userId)
        {
            return await _context.FavoriteBooks.CountAsync(x => x.UserId == userId);
        }

        public async Task Delete(int bookId, int userId)
        {
            var model = await _context.FavoriteBooks.FirstOrDefaultAsync(x => x.UserId == userId && x.BookId == bookId);
            _context.FavoriteBooks.Remove(model);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteByUserId(int userId)
        {
            var model = await _context.FavoriteBooks.Where(x => x.UserId == userId).ToListAsync();
            _context.FavoriteBooks.RemoveRange(model);
            await _context.SaveChangesAsync();
        }

        public async Task<List<FavoriteBook>> GetAll(int pageId)
        {
            return await _context.FavoriteBooks.Skip((pageId - 1) * 12).Take(12).ToListAsync();
        }

        public async Task Insert(FavoriteBook favoriteBook)
        {
            await _context.FavoriteBooks.AddAsync(favoriteBook);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> IsItemExist(FavoriteBook favoriteBook)
        {
            return await _context.FavoriteBooks.AnyAsync(x =>
                x.BookId == favoriteBook.BookId && x.UserId == favoriteBook.UserId);
        }

        public async Task<List<FavoriteBookDto>> GetFavoriteBookByUserId(int userId)
        {
            var result = new List<FavoriteBookDto>();
            var res = await _context.FavoriteBooks.Where(x => x.UserId == userId).Select(x => x.Book).ToListAsync();
            foreach (var item in res)
            {
                result.Add(new FavoriteBookDto()
                {
                    Book = item,
                    IsAvailable = await _context.UsersBook.AnyAsync(x => x.BookId == item.Id && x.EndDate == null)
                });
            }

            return result;
        }
    }
}
