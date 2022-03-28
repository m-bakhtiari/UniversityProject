using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection.PortableExecutable;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
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

        public async Task<Book> GetItem(int id)
        {
            return await _context.Books.FindAsync(id);
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
            if (book.UsableDays < 0)
            {
                return "تعداد روز در دسترس نامعتبر است";
            }
            book.IsDelete = false;
            book.IsAvailable = true;
            if (imgBook != null)
            {
                book.ImageName = NameGenerator.GenerateUniqCode() + Path.GetExtension(imgBook.FileName);
                var imagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Images/Books", book.ImageName);
                await using var stream = new FileStream(imagePath, FileMode.Create);
                await imgBook.CopyToAsync(stream);
            }

            var insert = await _context.AddAsync(book);
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
            _context.Update(book);
            await _context.SaveChangesAsync();
            return null;
        }
    }
}
