﻿using System.Collections.Generic;
using System.Threading.Tasks;
using UniversityProject.Data.Entities;

namespace UniversityProject.Core.Repositories
{
    public interface IUserBookRepository
    {
        Task<string> Insert(UserBook userBook);
        Task Update(UserBook userBook);
        Task<List<UserBook>> GetAll();
        Task<UserBook> GetItem(int id);
        Task Delete(int id);
    }
}