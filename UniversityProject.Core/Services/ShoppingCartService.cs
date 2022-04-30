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
    public class ShoppingCartService : IShoppingCartRepository
    {
        private readonly UniversityProjectContext _context;
        public ShoppingCartService(UniversityProjectContext context)
        {
            _context = context;
        }

        public async Task Delete(int bookId, int userId)
        {
            var model = await _context.ShoppingCarts.FirstOrDefaultAsync(x => x.UserId == userId && x.BookId == bookId);
            _context.ShoppingCarts.Remove(model);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteByUserId(int userId)
        {
            var model = await _context.ShoppingCarts.Where(x => x.UserId == userId).ToListAsync();
            _context.ShoppingCarts.RemoveRange(model);
            await _context.SaveChangesAsync();
        }

        public async Task<List<ShoppingCart>> GetAll(int pageId)
        {
            return await _context.ShoppingCarts.Skip((pageId - 1) * 12).Take(12).ToListAsync();
        }

        public async Task<string> Insert(ShoppingCart shoppingCart)
        {
            if (await _context.UsersBook.AnyAsync(x => x.BookId == shoppingCart.BookId && x.EndDate == null) == false)
            {
                await _context.ShoppingCarts.AddAsync(shoppingCart);
                await _context.SaveChangesAsync();
                return null;
            }

            return "کتاب در دسترس نمی باشد";
        }

        public async Task<int> CountByUserId(int userId)
        {
            return await _context.ShoppingCarts.CountAsync(x => x.UserId == userId);
        }

        public async Task<int> BookValidation(ShoppingCart shoppingCart)
        {
            if (await _context.UsersBook.AnyAsync(x => x.BookId == shoppingCart.BookId && x.EndDate == null))
            {
                return -1;
            }
            if (await _context.ShoppingCarts.AnyAsync(x => x.BookId == shoppingCart.BookId && x.UserId == shoppingCart.UserId))
            {
                return -2;
            }
            return 0;
        }

        public async Task<List<FavoriteBookDto>> GetShoppingCartByUserId(int userId)
        {
            var result = new List<FavoriteBookDto>();
            var res = await _context.ShoppingCarts.Where(x => x.UserId == userId).Select(x => x.Book).ToListAsync();
            foreach (var item in res)
            {
                result.Add(new FavoriteBookDto()
                {
                    Book = item,
                    IsAvailable = !await _context.UsersBook.AnyAsync(x => x.BookId == item.Id && x.EndDate == null)
                });
            }

            return result;
        }
    }
}
