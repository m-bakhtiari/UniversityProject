using System.Collections.Generic;
using System.Threading.Tasks;
using UniversityProject.Data.Entities;

namespace UniversityProject.Core.Repositories
{
    public interface ICategoryRepository
    {
        Task<string> Insert(Category category);
        Task<string> Update(Category category);
        Task Delete(int id);
        Task<List<Category>> GetAll();
        Task<Category> GetItem(int id);
        Task<List<Category>> GetMainGroups();
    }
}
