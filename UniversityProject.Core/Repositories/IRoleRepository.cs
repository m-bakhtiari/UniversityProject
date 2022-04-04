using System.Collections.Generic;
using System.Threading.Tasks;
using UniversityProject.Data.Entities;

namespace UniversityProject.Core.Repositories
{
    public interface IRoleRepository
    {
        Task<List<Role>> GetAll();
    }
}
