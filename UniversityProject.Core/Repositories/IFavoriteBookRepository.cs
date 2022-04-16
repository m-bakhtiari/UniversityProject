using System.Collections.Generic;
using System.Threading.Tasks;
using UniversityProject.Core.DTOs;
using UniversityProject.Data.Entities;

namespace UniversityProject.Core.Repositories
{
    public interface IFavoriteBookRepository
    {
        Task Insert(FavoriteBook favoriteBook);
        Task Delete(int bookId, int userId);
        Task<List<FavoriteBook>> GetAll(int pageId);
        Task DeleteByUserId(int userId);
        Task<bool> IsItemExist(FavoriteBook favoriteBook);
        Task<int> CountByUserId(int userId);
        Task<List<FavoriteBookDto>> GetFavoriteBookByUserId(int userId);
    }
}
