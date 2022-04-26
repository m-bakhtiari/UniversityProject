using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using UniversityProject.Data.Entities;

namespace UniversityProject.Core.Repositories
{
    public interface ISliderRepository
    {
        Task<List<Slider>> GetAll();
        Task<Slider> GetItem(int id);
        Task<string> Insert(Slider slider, IFormFile image);
        Task<string> Update(Slider slider, IFormFile image);
        Task Delete(int id);
        Task<int> SliderCount();
    }
}
