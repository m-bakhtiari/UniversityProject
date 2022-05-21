using System;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using UniversityProject.Core.DTOs;
using UniversityProject.Core.Repositories;
using UniversityProject.Core.Utils;
using UniversityProject.Data.Context;
using UniversityProject.Data.Entities;

namespace UniversityProject.Core.Services
{
    public class BookService : IBookRepository
    {
        private readonly UniversityProjectContext _context;

        public BookService(UniversityProjectContext context)
        {
            _context = context;
        }

        public async Task<int> BookCount()
        {
            return await _context.Books.CountAsync();
        }

        public async Task Delete(int id)
        {
            var book = await _context.Books.FindAsync(id);
            book.IsDelete = true;
            if (string.IsNullOrWhiteSpace(book.ImageName) == false)
            {
                var deleteImagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Images/Books", book.ImageName);
                if (File.Exists(deleteImagePath))
                {
                    File.Delete(deleteImagePath);
                }
            }
            await _context.SaveChangesAsync();
        }

        public async Task<List<Book>> GetAll()
        {
            return await _context.Books.ToListAsync();
        }

        public async Task<List<BookTitle>> GetAllTitles()
        {
            return await _context.Books.Select(x => new BookTitle()
            {
                Id = x.Id,
                Title = x.Title,
                LinkType = "Book"
            }).ToListAsync();
        }

        public async Task<BookDetailsDto> GetBookDetails(int bookId, int pageId)
        {
            var book = await _context.Books.FindAsync(bookId);
            var comment = await _context.Comments.Include(x => x.User)
                .Where(x => x.BookId == bookId && x.ParentId == null)
                .OrderByDescending(x => x.RecordDate).Skip((pageId - 1) * 12).Take(12).ToListAsync();
            var category = await _context.BookCategories.Where(x => x.BookId == bookId).Select(x => x.Category).ToListAsync();
            var res = new BookDetailsDto
            {
                AuthorName = book.AuthorName,
                Description = book.Description,
                BookId = book.Id,
                ShortDescription = book.ShortDescription,
                ImageName = book.ImageName,
                PublisherName = book.PublisherName,
                Title = book.Title,
                UsableDays = book.UsableDays,
                PageId = pageId,
                CountAll = await _context.Comments.CountAsync(x => x.BookId == bookId),
                Comments = comment,
                Categories = category,
                IsAvailable = !await _context.UsersBook.AnyAsync(x => x.BookId == bookId && x.EndDate == null),
                AddedDate = book.AddedDate.ToPersianMonth(),
                PublishDate = book.PublishDate.ToPersianMonth()
            };
            res.Answers = await _context.Comments.Where(x => res.Comments.Select(c => c.Id).Contains(x.ParentId.Value))
                .ToListAsync();
            return res;
        }

        public async Task<Book> GetItem(int id)
        {
            return await _context.Books.FindAsync(id);
        }

        public async Task<List<Book>> GetLatestBook()
        {
            return await _context.Books.OrderByDescending(x => x.PublishDate).Take(10).ToListAsync();
        }

        public async Task<LibraryDto> GetLibraryData(LibraryDto libraryDto)
        {
            IQueryable<Book> result = _context.Books.Include(x => x.BookCategories);
            if (libraryDto.CategoryIdSearch != null)
            {
                if (libraryDto.CategoryIdSearch.Any())
                {
                    var subGroup = await _context.Categories.Where(x => libraryDto.CategoryIdSearch.Contains(x.ParentId.Value))
                        .Select(x => x.Id).ToListAsync();
                    if (subGroup.Any())
                    {
                        libraryDto.CategoryIdSearch.AddRange(subGroup);
                    }
                    result = _context.BookCategories.Where(x => libraryDto.CategoryIdSearch.Contains(x.CategoryId)).Select(x => x.Book);
                }
            }
            if (string.IsNullOrWhiteSpace(libraryDto.EndPublishDate) == false)
            {
                result = result.Where(x => x.PublishDate <= Convert.ToDateTime(libraryDto.EndPublishDate));
            }
            if (string.IsNullOrWhiteSpace(libraryDto.StartPublishDate) == false)
            {
                result = result.Where(x => x.PublishDate >= Convert.ToDateTime(libraryDto.StartPublishDate));
            }
            if (string.IsNullOrWhiteSpace(libraryDto.EndDate) == false)
            {
                result = result.Where(x => x.AddedDate <= Convert.ToDateTime(libraryDto.EndDate));
            }
            if (string.IsNullOrWhiteSpace(libraryDto.StartDate) == false)
            {
                result = result.Where(x => x.AddedDate >= Convert.ToDateTime(libraryDto.StartDate));
            }
            if (string.IsNullOrWhiteSpace(libraryDto.Title) == false)
            {
                result = result.Where(x => x.Title.Contains(libraryDto.Title));
            }
            if (libraryDto.SortBy == "popularity")
            {
                result = result.OrderByDescending(x => x.UserBooks.Count);
            }
            else if (libraryDto.SortBy == "publishDate")
            {
                result = result.OrderByDescending(x => x.PublishDate);
            }
            else
            {
                result = result.OrderByDescending(x => x.AddedDate);
            }

            if (libraryDto.Authors != null)
            {
                result = result.Where(x => x.AuthorName.Contains(libraryDto.Authors));
            }
            if (libraryDto.Publishers != null)
            {
                result = result.Where(x => x.PublisherName.Contains(libraryDto.Publishers));
            }

            var notAvailable = new List<Book>();
            if (libraryDto.IsAvailable)
            {
                notAvailable = await _context.UsersBook.Where(x => x.EndDate == null).Select(x => x.Book)
                    .ToListAsync();
            }
            result = result.Distinct();
            var countAll = await result.CountAsync();
            var skip = (libraryDto.PageId - 1) * 12;
            result = result.Skip(skip).Take(12);
            var bookResult = await result.ToListAsync();
            if (notAvailable.Any())
            {
                bookResult = bookResult.Except(notAvailable).ToList();
            }

            var books = bookResult.Select(x => new BookDto()
            {
                AuthorName = x.AuthorName,
                Description = x.Description,
                Id = x.Id,
                ImageName = x.ImageName,
                IsDelete = x.IsDelete,
                Title = x.Title,
                ShortDescription = x.ShortDescription,
                UsableDays = x.UsableDays,
                IsAvailable = !_context.UsersBook.Any(u => u.BookId == x.Id && u.EndDate.HasValue == false)
            }).ToList();
            var res = new LibraryDto
            {
                Books = books,
                Categories = await _context.Categories.Include(x => x.BookCategories).ToListAsync(),
                CountAll = countAll,
                Title = libraryDto.Title,
                EndDate = libraryDto.EndDate,
                CategoryIdSearch = libraryDto.CategoryIdSearch,
                IsAvailable = libraryDto.IsAvailable,
                Publishers = libraryDto.Publishers,
                Authors = libraryDto.Authors,
                StartDate = libraryDto.StartDate,
                PageId = libraryDto.PageId,
                EndPublishDate = libraryDto.EndPublishDate,
                SortBy = libraryDto.SortBy,
                StartPublishDate = libraryDto.StartPublishDate,
            };
            return res;
        }

        public async Task<List<Book>> GetPopularBooks()
        {
            return await _context.Books.Include(x => x.UserBooks).OrderByDescending(x => x.UserBooks.Count).Take(10)
                .ToListAsync();
        }

        public async Task<string> Insert(Book book, IFormFile imgBook)
        {
            if (string.IsNullOrWhiteSpace(book.Title))
            {
                return "عنوان را وارد نمایید";
            }
            if (string.IsNullOrWhiteSpace(book.AuthorName))
            {
                return "نام نویسنده را وارد نمایید";
            }
            if (string.IsNullOrWhiteSpace(book.Description))
            {
                return "توضیحات را وارد نمایید";
            }
            if (book.UsableDays <= 0)
            {
                return "تعداد روز در دسترس نامعتبر است";
            }
            book.IsDelete = false;
            book.AddedDate = DateTime.Now;
            if (imgBook != null)
            {
                book.ImageName = NameGenerator.GenerateUniqCode() + Path.GetExtension(imgBook.FileName);
                var imagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Images/Books", book.ImageName);
                await using var stream = new FileStream(imagePath, FileMode.Create);
                await imgBook.CopyToAsync(stream);
            }

            var insert = await _context.Books.AddAsync(book);
            await _context.SaveChangesAsync();
            return insert.Entity.Id.ToString();
        }

        public async Task<string> Update(Book book, IFormFile imgBook)
        {
            if (string.IsNullOrWhiteSpace(book.Title))
            {
                return "عنوان را وارد نمایید";
            }
            if (string.IsNullOrWhiteSpace(book.AuthorName))
            {
                return "نام نویسنده را وارد نمایید";
            }
            if (string.IsNullOrWhiteSpace(book.Description))
            {
                return "توضیحات را وارد نمایید";
            }
            if (book.UsableDays <= 0)
            {
                return "تعداد روز در دسترس نامعتبر است";
            }
            book.IsDelete = false;
            if (imgBook != null)
            {
                if (string.IsNullOrWhiteSpace(book.ImageName) == false)
                {
                    var deleteImagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Images/Books", book.ImageName);
                    if (File.Exists(deleteImagePath))
                    {
                        File.Delete(deleteImagePath);
                    }
                }
                book.ImageName = NameGenerator.GenerateUniqCode() + Path.GetExtension(imgBook.FileName);
                var imagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Images/Books", book.ImageName);
                await using var stream = new FileStream(imagePath, FileMode.Create);
                await imgBook.CopyToAsync(stream);
            }
            _context.Books.Update(book);
            await _context.SaveChangesAsync();
            return null;
        }

    }
}
