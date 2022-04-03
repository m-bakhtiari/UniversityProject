using System.Collections.Generic;
using System.Threading.Tasks;
using UniversityProject.Data.Entities;

namespace UniversityProject.Core.Repositories
{
    public interface ICommentRepository
    {
        Task<string> Insert(Comment comment);
        Task<string> Update(Comment comment);
        Task Delete(int id);
        Task<List<Comment>> GetAll();
        Task<Comment> GetItem(int id);
        Task<List<Comment>> GetItemByBookId(int bookId, int pageId);
    }
}
