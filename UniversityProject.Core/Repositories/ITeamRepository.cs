using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using UniversityProject.Data.Entities;

namespace UniversityProject.Core.Repositories
{
    public interface ITeamRepository
    {
        Task Insert(Team team, IFormFile image);
        Task Update(Team team, IFormFile image);
        Task Delete(int id);
        Task<List<Team>> GetAll();
        Task<Team> GetItem(int id);
    }
}
