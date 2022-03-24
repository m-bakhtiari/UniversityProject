using System.Collections.Generic;
using UniversityProject.Data.Entities;

namespace UniversityProject.Core.DTOs
{
    public class CategoryListDto: PagingInfo
    {
        public List<Category> Categories { get; set; }
    }
}
