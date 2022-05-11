using System.Collections.Generic;
using System.Threading.Tasks;
using UniversityProject.Data.Entities;

namespace UniversityProject.Core.Repositories
{
    public interface IBookCategoryRepository
    {
        Task<string> Insert(BookCategory bookCategory);
        Task Delete(int bookId, int categoryId);
        Task<List<BookCategory>> GetAll();
        Task<BookCategory> GetItem(int bookId, int categoryId);
        Task<List<BookCategory>> GetItemByBookId(int bookId);
        Task DeleteByBookId(int bookId);
        Task<bool> IsExistBookByCategoryId(int categoryId);
    }
}
