using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using UniversityProject.Core.Repositories;
using UniversityProject.Core.Utils;
using UniversityProject.Data.Context;
using UniversityProject.Data.Entities;

namespace UniversityProject.Core.Services
{
    public class UserService : IUserRepository
    {
        private readonly UniversityProjectContext _context;
        public UserService(UniversityProjectContext context)
        {
            _context = context;
        }

        public async Task Delete(int id)
        {
            var user = await _context.Users.FindAsync(id);
            user.IsDelete = true;
            _context.Users.Update(user);
            await _context.SaveChangesAsync();
        }

        public async Task<List<User>> GetAll()
        {
            return await _context.Users.Include(x => x.Role).Include(x => x.SubscriptionType).ToListAsync();
        }

        public async Task<User> GetItem(int id)
        {
            return await _context.Users.FindAsync(id);
        }

        public async Task<string> Insert(User user)
        {
            if (string.IsNullOrWhiteSpace(user.Name))
            {
                return "نام را وارد نمایید";
            }
            if (string.IsNullOrWhiteSpace(user.Phone))
            {
                return "شماره تماس را وارد نمایید";
            }
            if (string.IsNullOrWhiteSpace(user.Password))
            {
                return "رمز عبور را وارد نمایید";
            }
            user.Password = PasswordHelper.EncodePasswordMd5(user.Password);
            user.RegisterTime = DateTime.Now;
            user.IsDelete = false;
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();
            return null;
        }

        public async Task<string> Update(User user)
        {
            if (string.IsNullOrWhiteSpace(user.Name))
            {
                return "نام را وارد نمایید";
            }
            if (string.IsNullOrWhiteSpace(user.Phone))
            {
                return "شماره تماس را وارد نمایید";
            }
            if (string.IsNullOrWhiteSpace(user.Password))
            {
                return "رمز عبور را وارد نمایید";
            }
            _context.Users.Update(user);
            await _context.SaveChangesAsync();
            return null;
        }
    }
}
