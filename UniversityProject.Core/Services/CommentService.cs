using System;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UniversityProject.Core.Repositories;
using UniversityProject.Data.Context;
using UniversityProject.Data.Entities;

namespace UniversityProject.Core.Services
{
    public class CommentService : ICommentRepository
    {
        private readonly UniversityProjectContext _context;
        public CommentService(UniversityProjectContext context)
        {
            _context = context;
        }

        public async Task<int> CommentCount()
        {
            return await _context.Comments.CountAsync();
        }

        public async Task<bool> CommentHasAnswer(int id)
        {
            return await _context.Comments.AnyAsync(x => x.ParentId == id);
        }

        public async Task Delete(int id)
        {
            var comment = await _context.Comments.FindAsync(id);
            var answer = await _context.Comments.Where(x => x.ParentId == id).ToListAsync();
            if (answer.Any())
            {
                _context.Comments.RemoveRange(answer);
            }
            _context.Comments.Remove(comment);
            await _context.SaveChangesAsync();
        }

        public async Task<List<Comment>> GetAll()
        {
            return await _context.Comments.Include(x => x.User)
                .Include(x => x.Book).OrderByDescending(x => x.RecordDate).ToListAsync();
        }

        public async Task<Comment> GetItem(int id)
        {
            return await _context.Comments.Include(x => x.User).FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<string> Insert(Comment comment)
        {
            if (string.IsNullOrWhiteSpace(comment.Text))
            {
                return "متن را وارد نمایید";
            }
            comment.RecordDate = DateTime.Now;
            await _context.Comments.AddAsync(comment);
            await _context.SaveChangesAsync();
            return null;
        }

        public async Task<string> Update(Comment comment)
        {
            if (string.IsNullOrWhiteSpace(comment.Text))
            {
                return "متن را وارد نمایید";
            }

            var model = await _context.Comments.FindAsync(comment.Id);
            model.Text = comment.Text;
            await _context.SaveChangesAsync();
            return null;
        }
    }
}
