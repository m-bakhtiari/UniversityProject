using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UniversityProject.Data.Entities;

namespace UniversityProject.Core.Repositories
{
    public interface IMessageRepository
    {
        Task<string> Insert(Message message);
        Task<List<Message>> GetAll();
        Task ToggleStatus(int messageId);
        Task<int> MessageCount();
        Task<int> UnreadMessageCount();
        Task<Message> GetItem(int id);
    }
}
