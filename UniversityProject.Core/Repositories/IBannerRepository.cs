using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Threading.Tasks;
using UniversityProject.Data.Entities;

namespace UniversityProject.Core.Repositories
{
    public interface IBannerRepository
    {
        Task<List<Banner>> GetAll();
        Task<Banner> GetItem(int id);
        Task<string> Insert(Banner slider, IFormFile image);
        Task<string> Update(Banner slider, IFormFile image);
        Task Delete(int id);
    }
}
