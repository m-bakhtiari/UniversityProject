using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using UniversityProject.Core.Repositories;
using UniversityProject.Data.Context;
using UniversityProject.Data.Entities;

namespace UniversityProject.Core.Services
{
    public class RoleService : IRoleRepository
    {
        private readonly UniversityProjectContext _context;

        public RoleService(UniversityProjectContext context)
        {
            _context = context;
        }

        public async Task Delete(int id)
        {
            var role = await _context.Roles.FindAsync(id);
            _context.Roles.Remove(role);
            await _context.SaveChangesAsync();
        }

        public async Task<List<Role>> GetAll()
        {
            return await _context.Roles.ToListAsync();
        }

        public async Task<Role> GetItem(int id)
        {
            return await _context.Roles.FindAsync(id);
        }

        public async Task<string> Insert(Role role)
        {
            if (string.IsNullOrWhiteSpace(role.Title))
            {
                return "عنوان را وارد نمایید";
            }
            await _context.Roles.AddAsync(role);
            await _context.SaveChangesAsync();
            return null;
        }

        public async Task<string> Update(Role role)
        {
            if (string.IsNullOrWhiteSpace(role.Title))
            {
                return "عنوان را وارد نمایید";
            }
            _context.Roles.Update(role);
            await _context.SaveChangesAsync();
            return null;
        }
    }
}
