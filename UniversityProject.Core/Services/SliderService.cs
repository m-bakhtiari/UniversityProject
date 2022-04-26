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
    public class SliderService : ISliderRepository
    {
        private readonly UniversityProjectContext _context;

        public SliderService(UniversityProjectContext context)
        {
            _context = context;
        }

        public async Task Delete(int id)
        {
            var slider = await _context.Sliders.FindAsync(id);
            var deleteImagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Images/Sliders", slider.Image);
            if (File.Exists(deleteImagePath))
            {
                File.Delete(deleteImagePath);
            }
            _context.Sliders.Remove(slider);
            await _context.SaveChangesAsync();
        }

        public async Task<List<Slider>> GetAll()
        {
            return await _context.Sliders.OrderBy(x => x.Position).ToListAsync();
        }

        public async Task<Slider> GetItem(int id)
        {
            return await _context.Sliders.FindAsync(id);
        }

        public async Task<string> Insert(Slider slider, IFormFile image)
        {
            if (image == null)
            {
                return "عکس را وارد نمایید";
            }
            slider.Image = NameGenerator.GenerateUniqCode() + Path.GetExtension(image.FileName);
            var imagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Images/Sliders", slider.Image);
            await using var stream = new FileStream(imagePath, FileMode.Create);
            await image.CopyToAsync(stream);
            await _context.Sliders.AddAsync(slider);
            await _context.SaveChangesAsync();
            return null;
        }

        public async Task<int> SliderCount()
        {
            return await _context.Sliders.CountAsync();
        }

        public async Task<string> Update(Slider slider, IFormFile image)
        {
            if (image != null)
            {
                var deleteImagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Images/Sliders", slider.Image);
                if (File.Exists(deleteImagePath))
                {
                    File.Delete(deleteImagePath);
                }
                slider.Image = NameGenerator.GenerateUniqCode() + Path.GetExtension(image.FileName);
                var imagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Images/Sliders", slider.Image);
                await using var stream = new FileStream(imagePath, FileMode.Create);
                await image.CopyToAsync(stream);
            }
            _context.Sliders.Update(slider);
            await _context.SaveChangesAsync();
            return null;
        }
    }
}
