using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using UniversityProject.Core.Repositories;
using UniversityProject.Core.Utils;
using UniversityProject.Data.Context;
using UniversityProject.Data.Entities;

namespace UniversityProject.Core.Services
{
    public class TeamService : ITeamRepository
    {
        private readonly UniversityProjectContext _context;

        public TeamService(UniversityProjectContext context)
        {
            _context = context;
        }

        public async Task Delete(int id)
        {
            var team = await _context.Teams.FindAsync(id);
            if (string.IsNullOrWhiteSpace(team.Image) == false)
            {
                var deleteImagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Images/Teams", team.Image);
                if (File.Exists(deleteImagePath))
                {
                    File.Delete(deleteImagePath);
                }
            }
            _context.Teams.Remove(team);
            await _context.SaveChangesAsync();
        }

        public async Task<List<Team>> GetAll()
        {
            return await _context.Teams.ToListAsync();
        }

        public async Task<Team> GetItem(int id)
        {
            return await _context.Teams.FindAsync(id);
        }

        public async Task Insert(Team team, IFormFile image)
        {
            if (image != null)
            {
                team.Image = NameGenerator.GenerateUniqCode() + Path.GetExtension(image.FileName);
                var imagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Images/Teams", team.Image);
                await using var stream = new FileStream(imagePath, FileMode.Create);
                await image.CopyToAsync(stream);
            }
            await _context.Teams.AddAsync(team);
            await _context.SaveChangesAsync();
        }

        public async Task Update(Team team, IFormFile image)
        {
            if (image != null)
            {
                if (string.IsNullOrWhiteSpace(team.Image) == false)
                {
                    var deleteImagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Images/Teams", team.Image);
                    if (File.Exists(deleteImagePath))
                    {
                        File.Delete(deleteImagePath);
                    }
                }
                team.Image = NameGenerator.GenerateUniqCode() + Path.GetExtension(image.FileName);
                var imagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Images/Teams", team.Image);
                await using var stream = new FileStream(imagePath, FileMode.Create);
                await image.CopyToAsync(stream);
            }
            _context.Teams.Update(team);
            await _context.SaveChangesAsync();
        }
    }
}
