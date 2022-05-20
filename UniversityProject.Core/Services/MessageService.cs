using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using UniversityProject.Core.Repositories;
using UniversityProject.Data.Context;
using UniversityProject.Data.Entities;

namespace UniversityProject.Core.Services
{
    public class MessageService : IMessageRepository
    {
        private readonly UniversityProjectContext _context;

        public MessageService(UniversityProjectContext context)
        {
            _context = context;
        }
        public async Task<List<Message>> GetAll()
        {
            return await _context.Messages.ToListAsync();
        }

        public async Task<Message> GetItem(int id)
        {
            return await _context.Messages.FindAsync(id);
        }

        public async Task<string> Insert(Message message)
        {
            if (string.IsNullOrWhiteSpace(message.Text))
            {
                return "متن را وارد نمایید";
            }
            message.ReadingStatus = false;
            message.RecordDate = DateTime.Now;
            await _context.Messages.AddAsync(message);
            await _context.SaveChangesAsync();
            return null;
        }

        public async Task<int> MessageCount()
        {
            return await _context.Messages.CountAsync();
        }

        public async Task ToggleStatus(int messageId)
        {
            var message = await _context.Messages.FindAsync(messageId);
            message.ReadingStatus = !message.ReadingStatus;
            await _context.SaveChangesAsync();
        }

        public async Task<int> UnreadMessageCount()
        {
            return await _context.Messages.CountAsync(x => x.ReadingStatus == false);
        }
    }
}
