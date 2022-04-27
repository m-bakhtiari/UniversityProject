using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
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
            return await _context.Users.Include(x => x.Role).ToListAsync();
        }

        public async Task<User> GetItem(int id)
        {
            return await _context.Users.FindAsync(id);
        }

        public async Task<User> GetItemByPhoneNumber(string phone)
        {
            return await _context.Users.FirstOrDefaultAsync(x => x.Phone == phone);
        }

        public async Task<bool> GetUserByCode(string phone, int code)
        {
            return await _context.Users.AnyAsync(x => x.Id == code && x.Phone == phone);
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
            if (await _context.Users.AnyAsync(x => x.Phone == user.Phone))
            {
                return "کاربری با این نام قبلا ثبت نام شده است";
            }
            user.Password = PasswordHelper.EncodePasswordMd5(user.Password);
            user.RegisterTime = DateTime.Now;
            user.IsDelete = false;
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();
            return null;
        }

        public async Task<User> LoginUser(User user)
        {
            return await _context.Users.FirstOrDefaultAsync(x => x.Phone == user.Phone && x.Password == user.Password);
        }

        public async Task<double> PenaltySum()
        {
            return await _context.Users.SumAsync(x => x.Penalty.Value);
        }

        public async Task ResetPassword(string phone, int code)
        {
            var user = await _context.Users.FirstOrDefaultAsync(x => x.Id == code && x.Phone == phone);
            user.Password = PasswordHelper.EncodePasswordMd5("1234");
            _context.Users.Update(user);
            await _context.SaveChangesAsync();
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

        public async Task<int> UserCount()
        {
            return await _context.Users.CountAsync();
        }
    }
}
