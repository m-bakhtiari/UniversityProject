using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using UniversityProject.Core.Repositories;
using UniversityProject.Core.Utils;
using UniversityProject.Data.Context;
using UniversityProject.Data.Entities;

namespace UniversityProject.Core.Services
{
    public class BannerService : IBannerRepository
    {
        private readonly UniversityProjectContext _context;

        public BannerService(UniversityProjectContext context)
        {
            _context = context;
        }

        public async Task<int> BannerCount()
        {
            return await _context.Banners.CountAsync();
        }

        public async Task Delete(int id)
        {
            var banner = await _context.Banners.FindAsync(id);
            var deleteImagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Images/Banners", banner.Image);
            if (File.Exists(deleteImagePath))
            {
                File.Delete(deleteImagePath);
            }
            _context.Banners.Remove(banner);
            await _context.SaveChangesAsync();
        }

        public async Task<List<Banner>> GetAll()
        {
            return await _context.Banners.OrderBy(x => x.Position).ToListAsync();
        }

        public async Task<Banner> GetItem(int id)
        {
            return await _context.Banners.FindAsync(id);
        }

        public async Task<string> Insert(Banner banner, IFormFile image)
        {
            if (image == null)
            {
                return "عکس را وارد نمایید";
            }
            banner.Image = NameGenerator.GenerateUniqCode() + Path.GetExtension(image.FileName);
            var imagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Images/Banners", banner.Image);
            await using var stream = new FileStream(imagePath, FileMode.Create);
            await image.CopyToAsync(stream);
            await _context.Banners.AddAsync(banner);
            await _context.SaveChangesAsync();
            return null;
        }

        public async Task<string> Update(Banner banner, IFormFile image)
        {
            if (image != null)
            {
                var deleteImagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Images/Banners", banner.Image);
                if (File.Exists(deleteImagePath))
                {
                    File.Delete(deleteImagePath);
                }
                banner.Image = NameGenerator.GenerateUniqCode() + Path.GetExtension(image.FileName);
                var imagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Images/Banners", banner.Image);
                await using var stream = new FileStream(imagePath, FileMode.Create);
                await image.CopyToAsync(stream);
            }
            _context.Banners.Update(banner);
            await _context.SaveChangesAsync();
            return null;
        }
    }
}
