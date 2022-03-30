using System.Collections.Generic;
using System.Threading.Tasks;
using UniversityProject.Data.Entities;

namespace UniversityProject.Core.Repositories
{
    public interface IRoleRepository
    {
        Task<string> Insert(Role role);
        Task<string> Update(Role role);
        Task Delete(int id);
        Task<Role> GetItem(int id);
        Task<List<Role>> GetAll();
    }
}
