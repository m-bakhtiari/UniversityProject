using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UniversityProject.Core.DTOs;
using UniversityProject.Domain.Entities;

namespace UniversityProject.Core.Repositories
{
    public interface ICategoryRepository
    {
        Task<string> Insert(Category category);
        Task<string> Update(Category category);
        Task Delete(int id);
        Task<List<Category>> GetAll();
        Task<Category> GetItem(int id);
        Task<CategoryListDto> GetItemsByPaging(CategorySearchDto dto);
    }
}
