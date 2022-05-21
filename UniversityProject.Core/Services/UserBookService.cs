using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using UniversityProject.Core.Repositories;
using UniversityProject.Data.Consts;
using UniversityProject.Data.Context;
using UniversityProject.Data.Entities;

namespace UniversityProject.Core.Services
{
    public class UserBookService : IUserBookRepository
    {
        private readonly UniversityProjectContext _context;
        public UserBookService(UniversityProjectContext context)
        {
            _context = context;
        }

        public async Task Delete(int id)
        {
            var model = await _context.UsersBook.FindAsync(id);
            _context.UsersBook.Remove(model);
            await _context.SaveChangesAsync();
        }

        public async Task<List<UserBook>> GetAll()
        {
            return await _context.UsersBook.Include(x => x.User).Include(x => x.Book)
                .OrderByDescending(x=>x.StartDate).ToListAsync();
        }

        public async Task<UserBook> GetItem(int id)
        {
            return await _context.UsersBook.FindAsync(id);
        }

        public async Task<List<UserBook>> GetItemByUserId(int userId)
        {
            return await _context.UsersBook.Include(x => x.Book).Where(x => x.UserId == userId).ToListAsync();
        }

        public async Task<string> Insert(UserBook userBook)
        {
            if (userBook.EndDate == new DateTime(0001, 01, 01, 0, 0, 0))
            {
                userBook.EndDate = null;
            }
            if (await _context.UsersBook.AnyAsync(x => x.BookId == userBook.BookId && x.EndDate == null))
            {
                return "کتاب در دسترس نمی باشد";
            }
            if (userBook.StartDate == new DateTime(0001, 01, 01, 0, 0, 0))
            {
                return "تاریخ شروع معتبر نمی باشد";
            }
            if (userBook.EndDate < userBook.StartDate)
            {
                return "تاریخ پایان معتبر نمی باشد";
            }
            var user = await _context.Users.FindAsync(userBook.UserId);
            var book = await _context.Books.FindAsync(userBook.BookId);
            if (userBook.EndDate.HasValue)
            {
                if ((userBook.EndDate.Value - userBook.StartDate).TotalDays > book.UsableDays)
                {
                    user.Penalty = ((userBook.EndDate.Value - userBook.StartDate).TotalDays) * Const.PenaltyPerDay;
                    _context.Users.Update(user);
                }
            }
            await _context.UsersBook.AddAsync(userBook);
            await _context.SaveChangesAsync();
            return null;
        }

        public async Task InsertList(List<UserBook> userBooks)
        {
            await _context.UsersBook.AddRangeAsync(userBooks);
            await _context.SaveChangesAsync();
        }

        public async Task<string> Update(UserBook userBook)
        {
            if (userBook.EndDate == new DateTime(0001, 01, 01, 0, 0, 0))
            {
                userBook.EndDate = null;
            }

            if (userBook.StartDate == new DateTime(0001, 01, 01, 0, 0, 0))
            {
                return "تاریخ شروع را وارد نمایید";
            }

            if (userBook.EndDate.HasValue)
            {
                if (userBook.EndDate < userBook.StartDate)
                {
                    return "تاریخ پایان معتبر نمی باشد";
                }
            }
          
            var book = await _context.Books.FindAsync(userBook.BookId);
            _context.UsersBook.Update(userBook);
            if (userBook.EndDate.HasValue)
            {
                if ((userBook.EndDate.Value - userBook.StartDate).TotalDays > book.UsableDays)
                {
                    var user = await _context.Users.FindAsync(userBook.UserId);
                    user.Penalty = ((userBook.EndDate.Value - userBook.StartDate).TotalDays) * Const.PenaltyPerDay;
                    _context.Users.Update(user);
                }
            }
            await _context.SaveChangesAsync();
            return null;
        }

        public async Task<int> UserBookCount()
        {
            return await _context.UsersBook.CountAsync();
        }

        public async Task<int> UserBookNotReturn()
        {
            return await _context.UsersBook.CountAsync(x => x.EndDate == null);
        }
    }
}
