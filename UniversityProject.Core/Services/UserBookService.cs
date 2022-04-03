using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using UniversityProject.Core.Repositories;
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
            return await _context.UsersBook.Include(x => x.User).Include(x => x.Book).ToListAsync();
        }

        public async Task<UserBook> GetItem(int id)
        {
            return await _context.UsersBook.FindAsync(id);
        }

        public async Task Insert(UserBook userBook)
        {
            await _context.UsersBook.AddAsync(userBook);
        }

        public async Task Update(UserBook userBook)
        {
            _context.UsersBook.Update(userBook);
            await _context.SaveChangesAsync();
        }
    }
}
