using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using UniversityProject.Core.Repositories;
using UniversityProject.Data.Context;
using UniversityProject.Data.Entities;

namespace UniversityProject.Core.Services
{
    public class SubscriptionTypeService : ISubscriptionTypeRepository
    {
        private readonly UniversityProjectContext _context;

        public SubscriptionTypeService(UniversityProjectContext context)
        {
            _context = context;
        }

        public async Task Delete(int id)
        {
            var sub = await _context.SubscriptionTypes.FindAsync(id);
            _context.SubscriptionTypes.Remove(sub);
            await _context.SaveChangesAsync();
        }

        public async Task<List<SubscriptionType>> GetAll()
        {
            return await _context.SubscriptionTypes.ToListAsync();
        }

        public async Task<SubscriptionType> GetItem(int id)
        {
            return await _context.SubscriptionTypes.FindAsync(id);
        }

        public async Task<string> Insert(SubscriptionType subscriptionType)
        {
            if (string.IsNullOrWhiteSpace(subscriptionType.Description))
            {
                return "توضیحات را وارد نمایید";
            }
            await _context.SubscriptionTypes.AddAsync(subscriptionType);
            await _context.SaveChangesAsync();
            return null;
        }

        public async Task<string> Update(SubscriptionType subscriptionType)
        {
            if (string.IsNullOrWhiteSpace(subscriptionType.Description))
            {
                return "توضیحات را وارد نمایید";
            }
            _context.SubscriptionTypes.Update(subscriptionType);
            await _context.SaveChangesAsync();
            return null;
        }
    }
}
