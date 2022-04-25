using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using UniversityProject.Core.DTOs;
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
        Task<List<BookTitle>> GetAllTitles();
        Task<List<Book>> GetLatestBook();
        Task<List<Book>> GetPopularBooks();
        Task<LibraryDto> GetLibraryData(LibraryDto libraryDto);
        Task<BookDetailsDto> GetBookDetails(int bookId,int pageId);
        Task<int> BookCount();
    }
}
