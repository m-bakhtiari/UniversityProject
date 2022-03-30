using System.Collections.Generic;
using System.Threading.Tasks;
using UniversityProject.Data.Entities;

namespace UniversityProject.Core.Repositories
{
    public interface ISubscriptionTypeRepository
    {
        Task<string> Insert(SubscriptionType subscriptionType);
        Task<string> Update(SubscriptionType subscriptionType);
        Task Delete(int id);
        Task<SubscriptionType> GetItem(int id);
        Task<List<SubscriptionType>> GetAll();
    }
}
