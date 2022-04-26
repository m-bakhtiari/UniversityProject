using System.Collections.Generic;
using System.Threading.Tasks;
using UniversityProject.Data.Entities;

namespace UniversityProject.Core.Repositories
{
    public interface IUserBookRepository
    {
        Task<string> Insert(UserBook userBook);
        Task InsertList(List<UserBook> userBooks);
        Task<string> Update(UserBook userBook);
        Task<List<UserBook>> GetAll();
        Task<UserBook> GetItem(int id);
        Task Delete(int id);
        Task<List<UserBook>> GetItemByUserId(int userId);
        Task<int> UserBookCount();
        Task<int> UserBookNotReturn();
    }
}
