using System.Collections.Generic;
using System.Threading.Tasks;
using UniversityProject.Data.Entities;

namespace UniversityProject.Core.Repositories
{
    public interface IUserRepository
    {
        Task<List<User>> GetAll();
        Task<User> GetItem(int id);
        Task<string> Insert(User user);
        Task<string> Update(User user);
        Task Delete(int id);
        Task<User> LoginUser(User user);
        Task<User> GetItemByPhoneNumber(string phone);
        Task<bool> GetUserByCode(string phone, int code);
        Task ResetPassword(string phone, int code);
    }
}
