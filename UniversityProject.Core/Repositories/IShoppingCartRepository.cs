using System.Collections.Generic;
using System.Threading.Tasks;
using UniversityProject.Core.DTOs;
using UniversityProject.Data.Entities;

namespace UniversityProject.Core.Repositories
{
    public interface IShoppingCartRepository
    {
        Task<string> Insert(ShoppingCart shoppingCart);
        Task Delete(int bookId, int userId);
        Task<List<ShoppingCart>> GetAll(int pageId);
        Task DeleteByUserId(int userId);
        Task<int> BookValidation(ShoppingCart shoppingCart);
        Task<int> CountByUserId(int userId);
        Task<List<FavoriteBookDto>> GetShoppingCartByUserId(int userId);
    }
}
