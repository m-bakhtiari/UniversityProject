using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using UniversityProject.Data.Entities;

namespace UniversityProject.Core.Repositories
{
    public interface IBookRepository
    {
        Task<string> Insert(Book book, IFormFile imgBook);
        Task<string> Update(Book book, IFormFile imgBook);
        Task Delete(int id);
        Task<List<Book>> GetAll();
        Task<Book> GetItem(int id);
    }
}
